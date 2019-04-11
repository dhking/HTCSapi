using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignApplication
{
    public  class Config
    {
        public static string ZQID = "ZQE26AEB7AA6364A6CB379A009DBEACEE4";
        public static string PRIVATE_KEY = @"MIICeAIBADANBgkqhkiG9w0BAQEFAASCAmIwggJeAgEAAoGBAKdLabq3lUHIMKi3QC
VE9gqRpo6niGBYydse3W82CPkszqjtm9JdpnXhK1AlTwTFJx8jdkepzldm4qqV/QpspLoB
clfQw68hkBX0T95ATRMUqRuY/j7nZiY72H9a+IRo6TTZ/XvD5Liw+RkLy9omCV9TOIjyU5
1eoGAxvmQrsdCzAgMBAAECgYBmOCIGQJ4mb5ervyymmRhtJMnMaHlfxWCxTo6moT
GibspnVafcRfSsGkVI10MM+xoIYLao2wyFQwxEhxjyAag0L1gC8mw7b2NRtHGAHWAEP
WSC/56RYh8bm5+wkr3IgeI9lwIdz57DOUPEOsbyap1tbwrQZGl2QJanVnU34roaAQJBA
PDIpwrgzONdrB1CMuKs5Bf2mw7EtuZdc7oGguHGiQJUeMGtj3oRz6MHmMmMs2xm
PSG06T+n6zh4ICpaUwnL6XMCQQCx3d45Zk+xZ/rB4o05Qv1EMQDverUiTMIw1rCwPI
W/JPalhZ5ipoZl5Dg7bJxE4IgEgY5deps/xkJZS0XWHavBAkEAquHMYxT9c9Mz5iPoFyUa
T8NtcgK7xyvFiN08H92VuLiYZuO1Mq3XTV2D2m5nm+PHONe6vbl/XzkposUtr4Mu7wJ
BAJfm5bybtf/Kz4r6EqYOogG04Bml8D/k0gunrqo0Zf0CcmqWHNgfY7RtHeESNrtUDpXa
l10aMrBaf5uG/5OHL4ECQQCa86ZUjpsFwVGPKsMCEi1jgK54JZiDI62ElM/7lLWHMo5yjl
7lt1rdCIsHx+pOnNdBfCdk9QjFyn3fvQLalDyP";
        public static string PUBLIC_KEY = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCFOiw5T+oetW2tmzcbj0CVJG+3y
rREvMkcnV6u7zIvr8wcnRsCq8GZjjxu60qdwJadl2OhFACIl1amnzMEzOFgkcDjunCkGDsI
ISgn5oWnKTN2Ok4RJlnH8T1hPQoGwoZY9Pfu+0+za0Znu7fjzk3xjbJ7yFwOdiLbWhIlxfy
ZEQIDAQAB";
        public static string URL = "http://signtest.zqsign.com/";
        public static string return_url = ConfigurationManager.AppSettings["weburl"];
        public static string notify_url =ConfigurationManager.AppSettings["ipurl"]+ "Zerp/notify";
    }
}
