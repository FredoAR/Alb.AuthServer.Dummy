
namespace Alb.AuthServer.Domain.Models.Identity
{
    public class AuthIdentityResultModel
    {
        private static readonly AuthIdentityResultModel _success = new AuthIdentityResultModel { Succeeded = true };
        protected readonly List<AuthIdentityErrorModel> _errors = new List<AuthIdentityErrorModel>();

        /// <summary>
        /// lo usare como bandera interna para indicar que salio bien
        /// </summary>
        public bool Succeeded { get; protected set; }


        /// <summary>
        /// Lo usare cuando todo salga ok
        /// </summary>
        public static AuthIdentityResultModel Success => _success;


        /// <summary>
        /// lo usare para los errores
        /// </summary>
        public IEnumerable<AuthIdentityErrorModel> Errors => _errors;



        #region methods

        public static AuthIdentityResultModel Failed(params AuthIdentityErrorModel[] errors)
        {
            var result = new AuthIdentityResultModel { Succeeded = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }
            return result;
        }
        

        #endregion
    }



    public class AuthIdentityResultModel<T> : AuthIdentityResultModel
    {
        private static readonly AuthIdentityResultModel<T> _success = new AuthIdentityResultModel<T> { Succeeded = true };


        public T Data { get; set; }


        public static AuthIdentityResultModel<T> Success => _success;


        #region methods

        public static AuthIdentityResultModel<T> Failed(params AuthIdentityErrorModel[] errors)
        {
            var result = new AuthIdentityResultModel<T> { Succeeded = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }
            return result;
        }
       

        #endregion

    }



}
