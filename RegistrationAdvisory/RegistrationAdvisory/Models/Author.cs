using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RegistrationAdvisory.Models
{
    //this class is included for demo purposes only
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple =true, Inherited =true)]
    public class Author : Attribute//class can also be named AuthorAttribute and referenced as [Author("","")]
    {
        public double version;
        private string Name;
        private string ID;

        public Author(string authorName, string authorId)
        {
            Name = authorName;
            ID = authorId;
            version = 1.0;
        }

        public double Version
        {
            set { this.version = value; }
        }


    }
}
