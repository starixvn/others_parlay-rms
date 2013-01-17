namespace ParlayRMS.Models
{
    public class Policy
    {
        private int permission = 0;

        public bool Writable
        {
            get
            {
                if (this.permission == 2)
                {
                    return true;
                }

                return false;
            }
        }

        public bool Readable
        {
            get
            {
                if (this.permission == 1 || this.permission == 2)
                {
                    return true;
                }

                return false;
            }
        }

        public Policy(int p)
        {
            this.permission = p;
        }
    }
}