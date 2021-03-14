using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security7
{
    public class CustomFile
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Text { get; set; }
        public TextType Type { get; set; }
    }

    public enum TextType
    {
        Crear,
        Encrypted
    }
}
