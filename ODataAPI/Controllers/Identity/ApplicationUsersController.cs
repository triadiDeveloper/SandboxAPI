using Application.DTOs.Identity;
using Domain.Entities.HumanResource;
using Domain.Entities.Identity;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.ActionFilters;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Application.Exceptions;
using MassTransit;
using Domain.BaseEntity;
using System.Security.Cryptography;
using WebAPI.Extensions;
using Infrastructure;

namespace WebAPI.Controllers.Identity
{
    public class ApplicationUsersController : ODataController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly RoleManager<ApplicationRole> _rolemanager;
        private readonly CleanDBContext _dbContext;
        protected DbSet<ApplicationUserRole> _role;
        protected DbSet<ApplicationUserCompany> _company;
        private readonly IConfigurationSection _jwtConfig;
        private readonly IConfigurationSection _azureConfig;

        /// <summary>
        /// This is function to add claims
        /// </summary>
        /// <param name="authClaims"></param>
        /// <returns></returns>
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig["Secret"]));

            var token = new JwtSecurityToken(
                //issuer: _jwtConfig["ValidIssuer"],
                //audience: _jwtConfig["ValidAudience"],
                expires: DateTime.Now.AddHours(20),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        public ApplicationUsersController(UserManager<ApplicationUser> user, RoleManager<ApplicationRole> role, IConfiguration configuration, CleanDBContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _usermanager = user;
            _rolemanager = role;
            _dbContext = dbContext;
            _jwtConfig = _configuration.GetSection("JwtSettings");
            _azureConfig = _configuration.GetSection("AzureAd");
            _httpContextAccessor = httpContextAccessor;
        }

        // Get ~/Users
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All, MaxExpansionDepth = 5)]
        [ServiceFilter(typeof(AuthorizeFilterAttribute))]
        public IActionResult Get()
        {
            return Ok(_usermanager.Users.AsQueryable());
        }

        // GET ~/Users(1)
        [EnableQuery]
        [ServiceFilter(typeof(AuthorizeFilterAttribute))]
        public async Task<IActionResult> Get(string key)
        {
            var book = await _usermanager.FindByIdAsync(key);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [ServiceFilter(typeof(AuthorizeFilterAttribute))]
        public void validationUser([FromBody] ApplicationUserDto fromBody, int formStatus)
        {
            if (fromBody == null)
                throw new BadRequestException("Data yang di kirim kosong!");
            // validation
            #region validate if 0 / null
            if (fromBody.ApplicationUserRoles == null) throw new BadRequestException("Hak Akses Harus Diisi");
            if (fromBody.ApplicationUserCompanies == null) throw new BadRequestException("Otorisasi Unit Kerja Harus Diisi");
            if (fromBody.FirstName == null) throw new BadRequestException("Nama Depan Harus Diisi");
            if (fromBody.LastName == null) fromBody.LastName = "";
            if (fromBody.PhoneNumber == null) fromBody.PhoneNumber = "";
            if (fromBody.IsAzureUser == false && (fromBody.UserName == null || fromBody.PasswordHash == null)) throw new BadRequestException("Jika Bukan User Azure, Maka Username dan Password Harus Diisi");
            #endregion


            if (fromBody.IsAzureUser == false && string.IsNullOrEmpty(fromBody.PasswordHash) == true && formStatus == 1) // New
                throw new BadRequestException("Password tidak boleh kosong bila bukan User Azure AD");
        }
        // POST ~/Users --> BODY JSON 
        [ServiceFilter(typeof(AuthorizeFilterAttribute))]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ApplicationUserDto fromBody)
        {
            try
            {
                validationUser(fromBody, 1); // 1 = New
                ApplicationRole? role = null;

                // Mapster
                var mapsterConfig = TypeAdapterConfig.GlobalSettings.Clone();
                mapsterConfig.Default.Ignore("Employee");
                var entity = fromBody.Adapt<ApplicationUser>(mapsterConfig);

                if (fromBody.EmployeeId != null)
                {
                    var user = await _dbContext.Users.Where(x => x.UserName == entity.UserName && x.EmployeeId == entity.EmployeeId)
                        .Select(s => new ApplicationUser()
                        {
                            Id = s.Id,
                            UserName = s.UserName,
                            EmployeeId = s.EmployeeId,
                            Employee = new Employee()
                            {
                                Id = s.Employee.Id,
                                Code = s.Employee.Code,
                                Name = s.Employee.Name,
                            }
                        })
                        .FirstOrDefaultAsync();
                    if (user != null)
                        throw new UniqueKeyException($"Pengguna ({fromBody.UserName}) tidak bisa menggunakan karyawan ({user.Employee.Code} - {user.Employee.Name}) karena telah digunakan di pengguna {user.UserName}.");
                }
                //

                Guid userid = NewId.NextSequentialGuid();
                var passwordHasher = String.Empty;
                string? passwordOwn = null;
                if (entity.PasswordHash != null)
                {
                    passwordHasher = _usermanager.PasswordHasher.HashPassword(entity, entity.PasswordHash);
                    passwordOwn = MobilePasswordHash(entity.PasswordHash);
                }
                else
                    passwordHasher = null;
                ApplicationUser _user = new ApplicationUser
                {
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Email = entity.Email,
                    PhoneNumber = entity.PhoneNumber,
                    PasswordHash = passwordHasher,
                    PasswordOwn = passwordOwn,
                    UserName = entity.UserName,
                    IsAzureUser = entity.IsAzureUser,
                    IsActive = entity.IsActive,
                    EmployeeId = entity.EmployeeId,
                    Id = userid
                };

                var identityResult = await _usermanager.CreateAsync(_user);

                // save role
                foreach (var roles in entity.ApplicationUserRoles)
                {
                    mapsterConfig = TypeAdapterConfig.GlobalSettings.Clone();
                    mapsterConfig.Default.Ignore("User");
                    var roleAdapt = roles.Adapt<ApplicationUserRole>(mapsterConfig);

                    role = await _rolemanager.FindByIdAsync(roleAdapt.RoleId.ToString());
                    if (role != null)
                    {
                        await _usermanager.AddToRoleAsync(_user, role.Name);
                    }
                }

                // save plant
                foreach (var Companies in entity.ApplicationUserCompanies)
                {
                    ApplicationUserCompany companyAdapt = new ApplicationUserCompany
                    {
                        ApplicationUserId = userid,
                        CompanyId = Companies.CompanyId
                    };
                    if (companyAdapt != null)
                    {
                        await _dbContext.ApplicationUserCompanies.AddAsync(companyAdapt);
                    }
                }

                await _dbContext.SaveChangesAsync();

                if (identityResult?.Succeeded == false)
                {
                    ModelState.AddModelError("UserId", errorMessage: identityResult?.Errors.FirstOrDefault()?.Description ?? "Failed to add User");
                    return BadRequest(ModelState);
                }

                return Created(entity);
            }
            catch (Exception e)
            {
                throw new BadRequestException(ModelState.ToString());
            }
        }

        // PUT ~/Users(1) --> BODY JSON 
        [ServiceFilter(typeof(AuthorizeFilterAttribute))]
        [HttpPut]
        public async Task<IActionResult> Put(string key, [FromBody] ApplicationUserDto fromBody)
        {
            try
            {
                // Mapster
                var mapsterConfig = TypeAdapterConfig.GlobalSettings.Clone();
                mapsterConfig.Default.Ignore("Employee");
                var _userDto = fromBody?.Adapt<ApplicationUser>(mapsterConfig);
                //

                validationUser(fromBody, 2); // 2 = edit

                //var entity = await _usermanager.FindByIdAsync(key.ToString());
                var entity = await _usermanager.FindByIdAsync(key) as ApplicationUser;
                if (entity == null)
                {
                    return NotFound();
                }

                var checkDuplicateUsername = await _dbContext.Users
                    .Where(x => x.UserName.ToLower() == fromBody.UserName.ToLower())
                    .Select(s => new ApplicationUser()
                    {
                        Id = s.Id,
                    })
                .FirstOrDefaultAsync();

                if (checkDuplicateUsername != null && (checkDuplicateUsername.Id != entity.Id))
                    throw new BadRequestException($"Username {fromBody.UserName} sudah ada");

                if (fromBody.EmployeeId != null)
                {
                    var userEmployee = await _dbContext.Users
                        .Where(x => x.EmployeeId == fromBody.EmployeeId)
                        .Select(s => new ApplicationUser()
                        {
                            Id = s.Id,
                            UserName = s.UserName,
                            EmployeeId = s.EmployeeId,
                            Employee = new Employee()
                            {
                                Id = s.Employee.Id,
                                Code = s.Employee.Code,
                                Name = s.Employee.Name,
                            }
                        })
                        .FirstOrDefaultAsync();

                    if (userEmployee != null && (userEmployee.Id != entity.Id))
                        throw new BadRequestException($"Pengguna ({fromBody.UserName}) tidak bisa menggunakan karyawan ({userEmployee.Employee.Code} - {userEmployee.Employee.Name}) karena telah digunakan di pengguna {userEmployee.UserName}.");
                }


                // save user data
                #region data user
                entity.FirstName = fromBody.FirstName;
                entity.LastName = fromBody.LastName;
                entity.PhoneNumber = fromBody.PhoneNumber;
                entity.Email = fromBody.Email;
                entity.UserName = fromBody.UserName;
                entity.IsActive = fromBody.IsActive;
                entity.IsAzureUser = fromBody.IsAzureUser;
                entity.EmployeeId = fromBody.EmployeeId;

                var passwordHasher = String.Empty;
                string? passwordOwn = null;

                //check if azure or not
                if (fromBody.IsAzureUser)
                {
                    entity.PasswordHash = null;
                }
                else
                {
                    if (_userDto.PasswordHash != string.Empty)
                    {
                        if (entity.PasswordHash == fromBody.PasswordHash)
                        {
                            passwordHasher = fromBody.PasswordHash;
                            passwordOwn = fromBody.PasswordOwn;

                        }
                        else
                        {
                            passwordHasher = _usermanager.PasswordHasher.HashPassword(entity, fromBody.PasswordHash);
                            passwordOwn = MobilePasswordHash(fromBody?.PasswordHash);
                        }
                    }
                    else
                    {
                        passwordHasher = fromBody.PasswordHash;
                        passwordOwn = fromBody.PasswordOwn;
                    }
                }




                //jika userazure
                entity.PasswordHash = passwordHasher;
                entity.PasswordOwn = passwordOwn;

                await _usermanager.UpdateAsync(entity);
                #endregion

                // updating role
                #region UserRole
                ApplicationRole? role = null;
                //var allRoleExisting = await _dbContext.UserRoles.Where(x => x.UserId == entity.Id).ToListAsync();
                //foreach (var _rolesExisting in allRoleExisting)
                //{
                //    //var roleAdapt = _rolesExisting.Adapt<ApplicationUserRole>();
                //    role = await _rolemanager.FindByIdAsync(_rolesExisting.RoleId.ToString());
                //    await _usermanager.RemoveFromRoleAsync(entity, role.Name.ToString());
                //}
                var listRoleIdExisting = _dbContext.UserRoles?.Where(x => x.UserId.Equals(entity.Id)).Select(s => s.RoleId).ToList();
                var RoleIdDelete = _userDto.ApplicationUserRoles.Where(x => x.UserId != Guid.Empty && x.UserId.Equals(entity.Id)).Select(s => s.RoleId).ToList();

                var listRoleToAdd = _userDto.ApplicationUserRoles?.Where(x => x.UserId == Guid.Empty || !listRoleIdExisting.Contains(x.RoleId)).ToList();
                var listRoleToUpdate = _userDto.ApplicationUserRoles?.Where(x => listRoleIdExisting.Contains(x.RoleId)).ToList();
                // looking for id stored that don't added at new data
                var listRoleToDelete = _dbContext.UserRoles?.Where(x => !RoleIdDelete.Contains(x.RoleId) && x.UserId.Equals(entity.Id)).ToList();

                // Add Data
                if (listRoleToAdd.Any())
                {
                    foreach (var item in listRoleToAdd)
                    {
                        //item.UserId = entity.Id;
                        //await _dbContext.UserRoles.AddAsync(item);
                        //await _dbContext.SaveChangesAsync();
                        mapsterConfig = TypeAdapterConfig.GlobalSettings.Clone();
                        mapsterConfig.Default.Ignore("User");
                        var roleAdapt = item.Adapt<ApplicationUserRole>(mapsterConfig);
                        role = await _rolemanager.FindByIdAsync(roleAdapt.RoleId.ToString());
                        if (role != null)
                        {
                            await _usermanager.AddToRoleAsync(entity, role.Name);
                        }
                    }
                }

                // update in case you could edit data later
                if (listRoleToUpdate.Any())
                {
                    foreach (var item in listRoleToUpdate)
                    {
                        mapsterConfig = TypeAdapterConfig.GlobalSettings.Clone();
                        mapsterConfig.Default.Ignore("User");
                        var roleAdapt = item.Adapt<ApplicationUserRole>(mapsterConfig);
                        role = await _rolemanager.FindByIdAsync(roleAdapt.RoleId.ToString());
                        if (role != null)
                            await _usermanager.AddToRoleAsync(entity, role.Name);
                    }
                }

                // delete role Data
                if (listRoleToDelete.Any())
                {
                    foreach (var item in listRoleToDelete)
                    {
                        //item.UserId = entity.Id;
                        //_dbContext.UserRoles.Remove(item);
                        //await _dbContext.SaveChangesAsync();

                        role = await _rolemanager.FindByIdAsync(item.RoleId.ToString());
                        if (role != null)
                            await _usermanager.RemoveFromRoleAsync(entity, role.Name.ToString());
                    }
                }
                #endregion

                // save plant
                #region UserPlant

                var listCompanyIdExisting = _dbContext.ApplicationUserCompanies?.Where(x => x.ApplicationUserId.Equals(entity.Id)).Select(s => s.CompanyId).ToList();
                var CompanyIdDelete = _userDto.ApplicationUserCompanies.Where(x => x.ApplicationUserId != Guid.Empty && x.ApplicationUserId.Equals(entity.Id)).Select(s => s.CompanyId).ToList();

                var listDataToAdd = _userDto.ApplicationUserCompanies?.Where(x => x.ApplicationUserId == Guid.Empty).ToList();
                var listDataToUpdate = _userDto.ApplicationUserCompanies?.Where(x => listCompanyIdExisting.Contains(x.CompanyId)).ToList();

                // looking for id stored that don't added at new data
                var listDataToDelete = _dbContext.ApplicationUserCompanies?.Where(x => !CompanyIdDelete.Contains(x.CompanyId) && x.ApplicationUserId.Equals(entity.Id)).ToList();

                // Add Data
                if (listDataToAdd.Any())
                {
                    foreach (var item in listDataToAdd)
                    {
                        item.ApplicationUserId = entity.Id;
                        item.Company = null;
                        item.ApplicationUser = null;
                        await _dbContext.ApplicationUserCompanies.AddAsync(item);
                    }
                }

                // delete Data
                if (listDataToDelete.Any())
                {
                    foreach (var item in listDataToDelete)
                    {
                        item.ApplicationUserId = entity.Id;
                        item.Company = null;
                        item.ApplicationUser = null;
                        _dbContext.ApplicationUserCompanies.Remove(item);
                    }
                }
                #endregion
                await _dbContext.SaveChangesAsync();

                return Updated(entity);
            }
            catch (Exception e)
            {
                //return BadRequest(e);
                throw new BadRequestException(e.Message.ToString());
            }
        }

        // PATCH ~/Users(1) --> BODY JSON (ONLY FIELD WAS CHANGED)
        [ServiceFilter(typeof(AuthorizeFilterAttribute))]
        public async Task<IActionResult> Patch([FromBody] string key, [FromBody] Delta<ApplicationUser> fromBody)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existing = await _usermanager.FindByIdAsync(key);
            if (existing == null)
            {
                return NotFound();
            }
            //fromBody.Patch(existing);

            try
            {
                await _usermanager.UpdateAsync(existing);
            }
            catch (Exception ex)
            {
                ModelState.Clear();

                if (ex.InnerException != null)
                {
                    if (ex.InnerException.Message.Contains("duplicate") == true)
                    {
                        ModelState.AddModelError("Code", "Code must be unique");
                    }
                }
                else
                {
                    ModelState.AddModelError(ex.HResult.ToString(), ex.Message);
                }

                return BadRequest(ModelState);
            }

            return Updated(existing);
        }

        // DELETE ~/Users(1) --> BODY JSON (ONLY FIELD WAS CHANGED)
        [ServiceFilter(typeof(AuthorizeFilterAttribute))]
        public async Task<IActionResult> Delete([FromBody] string key)
        {
            var entity = await _dbContext.Users.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }

            await _usermanager.DeleteAsync(entity);

            return NoContent();
        }

        [HttpPost]
        [ServiceFilter(typeof(AuthorizeFilterAttribute))]
        [Route($"{GlobalServiceRegister.RoutePrefix}/[controller]/AssignRole")]
        public async Task<IActionResult> AssignRole(ODataActionParameters parameters)
        {
            object? value;
            int paramUserId;
            int paramRoleId;

            if (!parameters.TryGetValue("UserId", out value)) return BadRequest();
            paramUserId = Convert.ToInt32(value);
            if (paramUserId <= 0)
            {
                ModelState.AddModelError("UserId", "Invalid UserId");
                return BadRequest(ModelState);
            }

            if (!parameters.TryGetValue("RoleId", out value)) return BadRequest();
            paramRoleId = Convert.ToInt32(value);
            if (paramUserId <= 0)
            {
                ModelState.AddModelError("RoleId", "Invalid RoleId");
                return BadRequest(ModelState);
            }

            var user = await _usermanager.FindByIdAsync(paramUserId.ToString());
            if (user == null)
            {
                ModelState.AddModelError("UserId", "UserId not found");
                return BadRequest(ModelState);
            }

            var role = await _rolemanager.FindByIdAsync(paramRoleId.ToString());
            if (role == null)
            {
                ModelState.AddModelError("RoleId", "RoleId not found");
                return BadRequest(ModelState);
            }

            if (role != null && user != null)
            {
                await _usermanager.AddToRoleAsync(user, role.Name);
            }

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route($"{GlobalServiceRegister.RoutePrefix}/[controller]/Login")]
        public async Task<IActionResult> Login(string user, string password)
        {
            ResponseModel response = new ResponseModel
            {
                Code = "0000",
                Name = ResponseCodes.Dict["0000"],
            };

            // check ke database untuk user yang dipassing, exist atau tidak
            var applicationuser = await _usermanager.FindByNameAsync(user);

            if (applicationuser != null && await _usermanager.CheckPasswordAsync(applicationuser, password))
            {
                if (!applicationuser.IsActive.GetValueOrDefault())
                {
                    response.Code = "U006";
                    response.Name = ResponseCodes.Dict["U006"];
                    response.Message = "User sudah tidak aktif";
                    return Unauthorized(response);
                }
                else
                {
                    if (applicationuser.EmployeeId != null)
                    {
                        var employee = await _dbContext.Employees.AsNoTracking().Where(s => s.Id == applicationuser.EmployeeId).FirstOrDefaultAsync();
                        if (employee != null)
                        {
                            // dicomment dulu
                            //if (employee.ResignationDate != null && (employee.ResignationDate <= DateTime.Today.Date))
                            //{
                            //    response.Code = "U007";
                            //    response.Name = ResponseCodes.Dict["U007"];
                            //    response.Message = $"User tersebut di assign pada Karyawan yang sudah resign pada tanggal {employee.ResignationDate.Value.ToString("dd-MMM-yyyy")} [{employee.Code} - {employee.Name}]";
                            //    return Unauthorized(response);
                            //}
                        }
                    }

                    var userroles = await _usermanager.GetRolesAsync(applicationuser);

                    var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, applicationuser.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.UtcNow).ToString()),
                    new Claim(JwtRegisteredClaimNames.Iss, _jwtConfig["ValidIssuer"]),
                    new Claim(JwtRegisteredClaimNames.Aud, _jwtConfig["ValidAudience"]),
                    new Claim("UserId", applicationuser.Id.ToString()),
                    new Claim("UserName", applicationuser.UserName),
                    new Claim("Email", string.IsNullOrEmpty(applicationuser.Email) ? "" : applicationuser.Email)
                };

                    foreach (var userrole in userroles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userrole));
                    }

                    var token = GetToken(authClaims);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
            }
            else if (applicationuser != null)
            {
                response.Code = "U005";
                response.Name = ResponseCodes.Dict["U005"];
                response.Message = "User dan Password yang dimasukan tidak cocok";
                return Unauthorized(response);
            }
            else
            {
                response.Code = "U005";
                response.Name = ResponseCodes.Dict["U005"];
                response.Message = "User dan Password yang dimasukan tidak cocok";
                return Unauthorized(response);
            }
        }

        /// <summary>
        /// Method ini untuk merubah token Azure menjadi Token Internal
        /// </summary>
        /// <param name="adtoken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route($"{GlobalServiceRegister.RoutePrefix}/[controller]/GetToken")]
        public async Task<IActionResult> GetToken(string adtoken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(adtoken);

            var clientId = jwtSecurityToken.Claims.First(claim => claim.Type == "appid").Value;
            var tenantId = jwtSecurityToken.Claims.First(claim => claim.Type == "tid").Value;
            var scope = jwtSecurityToken.Claims.First(claim => claim.Type == "scp").Value;
            var username = jwtSecurityToken.Claims.First(claim => claim.Type == "upn").Value;
            var exp = jwtSecurityToken.Claims.First(claim => claim.Type == "exp").Value;
            var expires = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(exp)).DateTime;
            var now = DateTime.UtcNow;

            if (now > expires)
            {
                ModelState.AddModelError("Expired", $"Your session has expired on {expires.AddHours(7)}.");
                return Unauthorized(ModelState);
            }

            if (scope != _azureConfig["Scope"])
            {
                ModelState.AddModelError("Invalid", "Invalid Azure Token.");
                return Unauthorized(ModelState);
            }

            if (clientId != _azureConfig["ClientId"])
            {
                ModelState.AddModelError("Invalid", "Invalid Azure Token.");
                return Unauthorized(ModelState);
            }

            if (tenantId != _azureConfig["TenantId"])
            {
                ModelState.AddModelError("Invalid", "Invalid Azure Token.");
                return Unauthorized(ModelState);
            }

            // check ke database untuk user yang dipassing, exist atau tidak
            var user = await _usermanager.FindByNameAsync(username);

            if (user != null)
            {
                var userroles = await _usermanager.GetRolesAsync(user);

                var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.UtcNow).ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, _jwtConfig["ValidIssuer"]),
                new Claim(JwtRegisteredClaimNames.Aud, _jwtConfig["ValidAudience"]),
                new Claim("UserId", user.Id.ToString()),
                new Claim("UserName", user.UserName),
                new Claim("Email", string.IsNullOrEmpty(user.Email) ? "" : user.Email)
            };

                foreach (var userrole in userroles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userrole));
                }
                var token = GetToken(authClaims);

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
            else
                return Unauthorized("User Tidak Ditemukan!");

            //return Unauthorized();
        }

        private string MobilePasswordHash(string toEncrypt, bool useHashing = true, string key = "12345678")
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            var reslt = Convert.ToBase64String(resultArray, 0, resultArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
    }
}