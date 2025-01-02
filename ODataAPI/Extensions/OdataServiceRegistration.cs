using Domain.Entities.Demography;
using Domain.Entities.HumanResource;
using Domain.Entities.Organization;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace WebAPI.Extensions
{
    public static class OdataServiceRegistration
    {
        public static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new();

            #region Demography

            builder.EntitySet<Country>("Countries").EntityType.Ignore(x => x.Version);

            #endregion

            #region HumanResource

            builder.EntitySet<Employee>("Employees").EntityType.Ignore(x => x.Version);

            #endregion

            #region Organization

            builder.EntitySet<Company>("Companies").EntityType.Ignore(x => x.Version);

            #endregion

            return builder.GetEdmModel();
        }
    }
}
