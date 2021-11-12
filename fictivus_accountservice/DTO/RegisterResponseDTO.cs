using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fictivus_accountservice.DTO
{
    public class RegisterResponseDTO
    {
        #region fields
        private string response;

        #endregion

        #region constructors
        //empty constructor for JSON
        public RegisterResponseDTO()
        {

        }

        public RegisterResponseDTO(string response)
        {
            this.response = response;
        }

        public string Response { get => response; set => response = value; }
        #endregion

        #region properties
        #endregion
    }
}
