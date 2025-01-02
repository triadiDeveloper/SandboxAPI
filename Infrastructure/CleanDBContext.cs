using Application.BaseEntity;
using Domain.BaseEntity;
using Domain.Entities.Demography;
using Domain.Entities.HumanResource;
using Domain.Entities.Identity;
using Domain.Entities.Organization;
using Domain.Entities.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;

namespace Infrastructure
{
    public sealed class CleanDBContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
    {
        private readonly string? _username;
        private readonly string? _ipAddress;

        public CleanDBContext(DbContextOptions<CleanDBContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _username = httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
            _ipAddress = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        }

        public async Task<List<T>> SelectDataAsync<T>(string query, bool IsQueryComplex)
        {
            List<T> result = new List<T>();
            var dt = await SelectData(query, IsQueryComplex);
            if (dt == null)
                return result;
            else
            {
                string JSONresult = JsonConvert.SerializeObject(dt);

                if (string.IsNullOrEmpty(JSONresult))
                    return result;
                else
                {
                    var convertResult = JsonConvert.DeserializeObject<List<T>>(JSONresult);
                    if (convertResult != null)
                        result = convertResult;

                    return result;
                }
            }
        }

        public async Task<bool> RunQueryAsync(string query)
        {
            try
            {
                if (!string.IsNullOrEmpty(query))
                {
                    await Database.ExecuteSqlRawAsync(query);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// https://stackoverflow.com/questions/1676753/sqldataadapter-vs-sqldatareader
        /// Sudah saya buktikan ketika query Payroll
        /// </summary>
        /// <param name="query"></param>
        /// <param name="IsQueryComplex"></param>
        /// <returns></returns>
        public async Task<DataTable> SelectData(string query, bool IsQueryComplex = true)
        {
            if (IsQueryComplex)
                return await SelectDataSqlDataAdapter(query);
            else
                return await SelectDataSqlDataReader(query);
        }

        /// <summary>
        /// Jika menggunakan query sederhana seperti select * from A, gunakan fungsi ini
        /// Karena hanya memakan memory sedikit
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<DataTable> SelectDataSqlDataReader(string query)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(Database.GetConnectionString()))
            {
                await conn.OpenAsync();

                using (SqlCommand command = new SqlCommand(query, conn))
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    command.CommandTimeout = 600;  // 10 Minutes
                    dt.Load(reader);
                }
            }
            return dt;
        }

        /// <summary>
        /// Jika menggunakan query kompleks dengan banyak join dan subquery, gunakan fungsi ini
        /// Karena hanya memakan memory sedikit
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<DataTable> SelectDataSqlDataAdapter(string query)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(Database.GetConnectionString()))
            {
                await conn.OpenAsync();

                using (SqlDataAdapter ad = new SqlDataAdapter(query, conn))
                {
                    // set the CommandTimeout
                    ad.SelectCommand.CommandTimeout = 600;  // 10 Minutes
                    ad.Fill(dt);
                }
            }

            return dt;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            // Get audit entries
            var auditEntries = OnBeforeSaveChanges();

            // Save current entity
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            // Save audit entries
            if (auditEntries.Count > 0) await OnAfterSaveChangesAsync(auditEntries);

            return result;
        }

        private List<AuditEntry> OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();

            foreach (var entry in ChangeTracker.Entries())
            {
                // Dot not audit entities that are not tracked, not changed, or not of type IAuditable
                if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                if (entry.Entity is IAudited == true)
                {

                    //mencegah created date atau created user berubah ketika update
                    var creationUser = entry.Properties.Where(x => x.Metadata.Name == "CreatedUser").FirstOrDefault();
                    if (creationUser != null)
                    {
                        creationUser.IsModified = false;
                    }

                    var creationDate = entry.Properties.Where(x => x.Metadata.Name == "CreatedDate").FirstOrDefault();
                    if (creationDate != null)
                    {
                        creationDate.IsModified = false;
                    }

                    if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                    {
                        if (entry.State == EntityState.Added)
                        {
                            var propCreatedUser = entry.Properties.Where(x => x.Metadata.Name == "CreatedUser").FirstOrDefault();
                            if (propCreatedUser != null)
                            {
                                var createdUser = propCreatedUser.CurrentValue as string;
                                if (string.IsNullOrEmpty(createdUser))
                                    propCreatedUser.CurrentValue = _username;
                            }
                            var propCreatedDate = entry.Properties.Where(x => x.Metadata.Name == "CreatedDate").FirstOrDefault();
                            if (propCreatedDate != null)
                            {
                                propCreatedDate.CurrentValue = DateTime.Now;
                            }
                        }
                        else if (entry.State == EntityState.Modified)
                        {
                            var propModifiedUser = entry.Properties.Where(x => x.Metadata.Name == "ModifiedUser").FirstOrDefault();
                            if (propModifiedUser != null)
                            {
                                var modifiedUser = propModifiedUser.CurrentValue as string;
                                if (string.IsNullOrEmpty(modifiedUser) || (!string.IsNullOrEmpty(modifiedUser) && modifiedUser != _username && modifiedUser != "Unauthenticated user"))
                                    propModifiedUser.CurrentValue = _username;
                            }
                            var propModifiedDate = entry.Properties.Where(x => x.Metadata.Name == "ModifiedDate").FirstOrDefault();
                            if (propModifiedDate != null)
                            {
                                propModifiedDate.CurrentValue = DateTime.Now;
                            }
                        }
                    }
                }

                if (entry.Entity is IAuditable == true && entry.Entity is INotAuditable == false && entry.State != EntityState.Added)
                {

                    var auditEntry = new AuditEntry(entry);
                    auditEntry.TableName = entry.Entity.GetType().Name;
                    auditEntry.UserName = _username;
                    auditEntry.IpAddress = _ipAddress;
                    auditEntry.EntityName = entry.Metadata.ClrType.Name;
                    auditEntries.Add(auditEntry);

                    foreach (var property in entry.Properties)
                    {
                        string propertyName = property.Metadata.Name;

                        if (property.Metadata.IsPrimaryKey())
                        {
#pragma warning disable CS8601 // Possible null reference assignment.
                            auditEntry.KeyValues[propertyName] = property.CurrentValue;
#pragma warning restore CS8601 // Possible null reference assignment.
                            continue;
                        }
                        switch (entry.State)
                        {
                            case EntityState.Added:
                                auditEntry.ActionType = "INSERT";
#pragma warning disable CS8601 // Possible null reference assignment.
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
#pragma warning restore CS8601 // Possible null reference assignment.
                                break;
                            case EntityState.Deleted:
                                auditEntry.ActionType = "DELETE";
#pragma warning disable CS8601 // Possible null reference assignment.
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
#pragma warning restore CS8601 // Possible null reference assignment.
                                break;
                            case EntityState.Modified:
                                if (property.IsModified)
                                {
                                    auditEntry.ChangedColumns.Add(propertyName);
                                    auditEntry.ActionType = "UPDATE";
#pragma warning disable CS8601 // Possible null reference assignment.
                                    auditEntry.OldValues[propertyName] = property.OriginalValue;
#pragma warning restore CS8601 // Possible null reference assignment.

#pragma warning disable CS8601 // Possible null reference assignment.
                                    auditEntry.NewValues[propertyName] = property.CurrentValue;
#pragma warning restore CS8601 // Possible null reference assignment.
                                }
                                break;
                        }
                    }
                }
            }

            return auditEntries;
        }

        private Task OnAfterSaveChangesAsync(List<AuditEntry> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0)
                return Task.CompletedTask;

            // For each temporary property in each audit entry - update the value in the audit entry to the actual (generated) value
            foreach (var entry in auditEntries)
            {
                AuditEntries.Add(entry.ToAudit());
            }

            return SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // define DeleteBehaviour
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }

            // apply configuration dengan membaca assembly 
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CleanDBContext).Assembly);
        }

        #region Schema Demography

        public DbSet<Country> Countries { get; set; }

        #endregion

        #region Schema HumanResource

        public DbSet<Employee> Employees { get; set; }

        #endregion

        #region Schema Identity

        public DbSet<ApplicationController> ApplicationControllers { get; set; }
        public DbSet<ApplicationControllerMethod> ApplicationControllerMethods { get; set; }
        public DbSet<ApplicationEndpoint> ApplicationEndpoints { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationRoleClaim> ApplicationRoleClaims { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationUserApproval> ApplicationUserApprovals { get; set; }
        public DbSet<ApplicationUserApprovalDetail> ApplicationUserApprovalDetails { get; set; }
        public DbSet<ApplicationUserClaim> ApplicationUserClaims { get; set; }
        public DbSet<ApplicationUserCompany> ApplicationUserCompanies { get; set; }
        public DbSet<ApplicationUserInfo> ApplicationUserInfos { get; set; }
        public DbSet<ApplicationUserLogin> ApplicationUserLogins { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public DbSet<ApplicationUserToken> ApplicationUserTokens { get; set; }
        public DbSet<ApplicationNavigation> ApplicationNavigations { get; set; }
        public DbSet<ApplicationNavigationRole> ApplicationNavigationRoles { get; set; }

        #endregion

        #region Schema Organization

        public DbSet<Company> Companies { get; set; }

        #endregion

        #region Schema Shared

        public DbSet<AuditTrail> AuditEntries { get; set; } = default!;

        #endregion
    }
}