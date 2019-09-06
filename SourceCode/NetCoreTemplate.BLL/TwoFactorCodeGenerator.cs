using OtpNet;
using System;
namespace NetCoreTemplate.BLL
{
    public class TwoFactorCodeGenerator
    {

        private readonly Totp _timedOtp;
        private readonly int _timeout;
        public TwoFactorCodeGenerator(byte[] secretKey, int passwordLenght = 6, int timeOutSecs = 300)
        {
            _timedOtp = new Totp(
                secretKey,
                totpSize: passwordLenght,                               // length of otp
                step: timeOutSecs,                                          //new code every 5 minutes
                mode: OtpHashMode.Sha512,
                timeCorrection: new TimeCorrection(DateTime.Now));
            _timeout = timeOutSecs;
        }

        public int Timeout => _timeout;

        public string GenerateCode() { return _timedOtp.ComputeTotp(); }
        public bool ValidateCode(string password) {
            VerificationWindow window = new VerificationWindow(previous:1, future:1);
            long timeWindowUsed;

            return _timedOtp.VerifyTotp(DateTime.Now,password, out timeWindowUsed);
        }
    }
}
