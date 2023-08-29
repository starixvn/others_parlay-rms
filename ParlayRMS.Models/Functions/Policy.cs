namespace ParlayRMS.Models.Functions
{
    public class Policy
    {
        private readonly int _permission;

        public Policy(int p)
        {
            _permission = p;
        }

        public bool Writable
        {
            get
            {
                if (_permission == 2)
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
                if (_permission == 1 || _permission == 2)
                {
                    return true;
                }

                return false;
            }
        }
    }
}
