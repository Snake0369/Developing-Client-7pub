using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Bot.Service.Services
{
    public class HttpClientRequest : IHttpClientRequest
    {
        private readonly HttpClient _httpClient;
        private readonly string _token = "03AGdBq25UhzQ5ChdRg_E31kmvMb3-hkBUnjOEf925xOiv2UqAcbcgXsUucRUvg4hGSZLCG7CtfMRMFmQOJgaLOAJ2GEvC7TbDrSGZ7tza59q24GmlHTS7dgfZfhOaeXr4VT_PamZeCTYEdp8vDhGDGJY5zQJ3mLsWS5eLzgGlU-1rbK-Ynn7i1CJOUNwb3oJzUN8B2oM24PEBvjEyNLLlAhn04fqE1xGJeZ-2QYRWMzGXtBvADiVZL6L9YowfFrtRi4PRlHfQCupZRhJtpzcMbkc9U8gbarZQchOMKYTy9rAPQtDGq_DLY9bSOVCKOsKkZ6or9lyWo9cOqD1dA3xgOiSHGDpN7Yqxf8VVrf2MGBCUtR4ofgHFQ5CH8cH138aasvynM63Y54XQ6x6YAI9Sx0qsmNoeL5GTdlVEXyMAE4SdJdvkpCIoh7xxxC6j4qKVOAOtdi1zkyky";
        public HttpClientRequest(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.Timeout = new TimeSpan(0, 10, 0);
        }

        public async Task<string> GetTradingFile(string market, string code, string fileCode, string fileName, DateTime date)
        {
            var path = $"export9.out?market={market}&em={code}&" +
                $"{_token}&code={fileCode}&apply=0&df={date.Day}&mf={date.Month - 1}&yf={date:yyyy}&from={date:dd.MM.yyyy}&dt={date.Day}&mt={date.Month - 1}&yt={date:yyyy}&to={date:dd.MM.yyyy}&p=1&f={fileName}&e=.txt&cn={fileCode}&dtf=1&tmf=1&MSOR=1&mstime=on&mstimever=1&sep=1&sep2=1&datf=6";
            var message = new HttpRequestMessage(HttpMethod.Get, path);
            var response = await _httpClient.SendAsync(message);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            throw new Exception($"{response.StatusCode}, {response.ReasonPhrase}");
        }
    }
}
