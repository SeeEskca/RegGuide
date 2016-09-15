using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationAdvisory.Models
{
    public class GenericFactory <T,I> where T:I
    {
        GenericFactory() { }
        public static I createInstanceOf(params object[] args)
        {
            return (I)Activator.CreateInstance(typeof(T), args);
        }
    }
}
