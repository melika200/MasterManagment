using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MasterManagment.Application.Contracts.Product
{

    public class EditProductCommand : CreateProductCommand
    {

        [JsonIgnore]
        public long Id { get; set; }
      
    }

}
