using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACCEA.EmailSpooler.Entities
{
    public class Email
    {        
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string AttachmentName { get; set; }
        public byte[] AttachmentData { get; set; }
    }
}
