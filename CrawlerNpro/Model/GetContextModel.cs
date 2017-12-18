using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerNpro.Model
{
   public class GetContextModel
    {

        private string state;
        private bool flag;
        private string total;
        private string fullPageSIze;
        private string error;

        public string State
        {
            get
            {
                return state;
            }

            set
            {
                state = value;
            }
        }

        public string Total
        {
            get
            {
                return total;
            }

            set
            {
                total = value;
            }
        }

        public string FullPageSIze
        {
            get
            {
                return fullPageSIze;
            }

            set
            {
                fullPageSIze = value;
            }
        }

        public string Error
        {
            get
            {
                return error;
            }

            set
            {
                error = value;
            }
        }

        public bool Flag
        {
            get
            {
                return flag;
            }

            set
            {
                flag = value;
            }
        }
    }
}
