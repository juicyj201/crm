using System;
using System.Net.Http;
using System.ComponentModel.Design;
using Microsoft.AspNetCore.Mvc;
using API.Helper;
using System.Numerics;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialController : ControllerBase
    {
        #region Var setups
        private Random rand = new Random();
        private readonly string csrfState;
        #endregion

        internal readonly Helper.Helper helper;
        public SocialController(IConfiguration config)
        {
            csrfState = rand.Next(36).ToString().Substring(2);
            helper = new Helper.Helper(config);
        }

        #region Tiktok
        internal string buildCodeVerifier(){
            var minLength = 43;
            var maxLength = 128;
            var length = rand.Next(43, 128);

            var result = "";
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~";
            var characterLength = characters.Length;
            for(var i = 0; i < length; i++){
                result += characters.ElementAtOrDefault(new Index(int.Parse(Math.Ceiling(((double) (rand.Next() * characterLength))).ToString()), false));
            }

            return result;
        }

        [HttpGet("Tiktok")]
        public string GetTiktok(){
            return "TIKTOK";
        }

        [HttpGet("Auth/Tiktok")]
        public async Task<ActionResult<string>> AuthTiktok(){
            #region task 1
            string codeVerifier = buildCodeVerifier();
            var encoded = System.Text.Encoding.UTF8.GetBytes(codeVerifier);
            string codeChallenge = null;

            using(SHA256 sha = SHA256.Create()){
                var hash = sha.ComputeHash(encoded);

                foreach(var item in hash){
                    codeChallenge += item.ToString();
                }
            }

            var url = helper.TiktokInfo((int) API.Helper.TiktokThings.TIKTOK_AUTH_URL);

            url += "?client_key={helper.ApiInfo(TiktokThings.TIKTOK_CLIENT_KEY)}";
            url += "&scope=user.info.basic";
            url += "&response_type=code";
            url += "&redirect_uri={helper.ApiInfo(Info.SERVER_API_URL)}";
            url += "&state=" + csrfState;
            url += "&code_challenge={codeChallenge.ToString()}";
            url += "&code_challenge_method=S256";
            #endregion

            #region task 2
            HttpClient client = new HttpClient();
            // client.BaseAddress = new Uri(url);  
            using (var result = await client.GetAsync(url)){
                if(result.IsSuccessStatusCode){
                    return result.Content.R
                }
            }

            return helper.TiktokScope();
            #endregion
        }
        #endregion
    }
}