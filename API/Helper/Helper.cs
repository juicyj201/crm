using System.Configuration;

namespace API.Helper
{
    public class Helper
    {
        private readonly IConfiguration _config;

        public Helper(IConfiguration config)
        {
            _config = config;
        }

        public string ApiInfo(int i){
            switch(i){
                case 0:
                    return (_config.GetSection("Api:ServerApiUrl")).Value;
                case 1:
                    return (_config.GetSection("Api:ClientApiUrl")).Value;
                default:
                    break;     
            }

            return null;
        }

        public string TiktokInfo(int i) {
            switch(i){
                case 0:
                    return (_config.GetSection("Tiktok:AuthUrl")).Value;
                case 1:
                    return (_config.GetSection("Tiktok:ClientKey")).Value;
                case 2:
                    return (_config.GetSection("Tiktok:ClientSecret")).Value;
                default:
                    break;          
            }

            return null;
        }
    }

    public enum TiktokThings{
        TIKTOK_AUTH_URL = 0,
        TIKTOK_CLIENT_KEY = 1,
        TIKTOK_CLIENT_SECRET = 2
    }

    public enum Info{
        SERVER_API_URL = 0,
        CLIENT_API_URL = 1
    }
}