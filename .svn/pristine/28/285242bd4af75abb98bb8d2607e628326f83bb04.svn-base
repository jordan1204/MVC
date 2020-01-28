using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Data.Entity.Validation;

namespace CommonResource.Models
{
    public partial class ebWebsiteEntities
    {
        #region Common Method
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public void SaveChangesWithoutValidation()
        {
            try
            {
                base.Configuration.ValidateOnSaveEnabled = false;
                base.SaveChanges();
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
            finally
            {
                base.Configuration.ValidateOnSaveEnabled = true;
            }
        }
        #endregion
    }
}
