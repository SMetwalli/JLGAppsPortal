using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JLGProcessPortal.Models
{
    public class Mailboxes
    {
      
        public string Mailbox { get; set; }
        public int Index { get; set; }
    }


    public class MailboxSelection
    {
        public List<Mailboxes> GetMailboxes()
        {
            var mailBoxes = new List<Mailboxes>
            {
                new Mailboxes{ Index=7,  Mailbox="tvm_updates@johnsonlawgroup.com"},
                new Mailboxes{ Index=10, Mailbox="hips_updates@johnsonlawgroup.com"},
                new Mailboxes{ Index=7,  Mailbox="essure_updates@johnsonlawgroup.com"},
                new Mailboxes{ Index=7,  Mailbox="ethicontvt@johnsonlawgroup.com"},
                new Mailboxes{ Index=5,  Mailbox="talc_updates@johnsonlawgroup.com"},
                new Mailboxes{ Index=8,  Mailbox="txt_updates@johnsonlawgroup.com"},
                new Mailboxes{ Index=4,  Mailbox="rup_updates@johnsonlawgroup.com"},
                new Mailboxes{ Index=3,  Mailbox="rsp_updates@johnsonlawgroup.com"},
                new Mailboxes{ Index=5,  Mailbox="xar_updates@johnsonlawgroup.com"},                
                new Mailboxes{ Index=9,  Mailbox="zan_updates@johnsonlawgroup.com"},
                new Mailboxes{ Index=2,  Mailbox="herniamesh@johnsonlawgroup.com"},
                new Mailboxes{ Index=1,  Mailbox="earplug_updates@johnsonlawgroup.com"},
                new Mailboxes{ Index=6,  Mailbox="tdf_updates@johnsonlawgroup.com"}
            };
            return mailBoxes;
        }
    }
}
