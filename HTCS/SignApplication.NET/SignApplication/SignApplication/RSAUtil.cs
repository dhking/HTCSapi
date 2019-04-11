using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SignApplication
{
    public  class RSAUtil
    {
        /*static void Main(string[] args)
        {
            string private_key = "MIICdQIBADANBgkqhkiG9w0BAQEFAASCAl8wggJbAgEAAoGBAIc0R8mA+IKdbTToUmalO1R5UZGAWgi7I6uEn13c7QW6JOodIL0V1cTiuWLsjif91QHdXkw9LBBdmoZvLU2XM8wWdTakcdL+MraKZ9qWU7KHJS/pruQLVrqwCftWnekJTodscdnyy5EyXbeRHuRQBYX/FvJ1BsSOKbGAPw+fY+3zAgMBAAECgYB97UOvel/7x4SfcoLM97h6xUQjsNgqqaJfbfbBBbP9UZJwOxDzDBM+fa0NZiZBOTnbssLRX2hbFdOGwqX/ToT+gO/WnYxbdm6lnOX5aujdQLcnLIEtODrVfkxCBgvoW7voAISwDSP83En2Tgvdkk0CchaJ2wlVGa75TDtDeCK+AQJBAN7lk5fts+Ep7kjnUDEhWcub/ag4llTJvEaRYsWBQoUaVK5oGHrhNtUeaJEOtCVH5QFwa2dRXc8LmVBlT51OIkMCQQCbSK1CdHLY0JDqBIG8OGs2hKrTT2jq7MKVniwvWcnZEm3xUbA3E5RoW52LfssMs3nwupvILQvgLrlBaZ8LygKRAkB1t+G/N3bsz+xc7G1ZxTdbZUMN+PTMSs74pgf4L1AmY8WdZrSnERKYc7reAVn65oF3xRu0MTDODF4oK5lkhsNpAkAXILb1e+STGFVNFYjBIOIPB6ltuZkVzFea8yj/kG74zr7jP1hwi5ECDgsj+KmDZcPWr+R95v+qzyq2bGXM2rSRAkBrXRJL08UBP0kLt41R3lEMaCijtlxwiOrkrWkAHQ2YzKYw0qGsjx/y0BM/TsbbrMbMebPxrNF0xsr+nNw5tirG";
            string public_key = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCHNEfJgPiCnW006FJmpTtUeVGRgFoIuyOrhJ9d3O0FuiTqHSC9FdXE4rli7I4n/dUB3V5MPSwQXZqGby1NlzPMFnU2pHHS/jK2imfallOyhyUv6a7kC1a6sAn7Vp3pCU6HbHHZ8suRMl23kR7kUAWF/xbydQbEjimxgD8Pn2Pt8wIDAQAB";


            Dictionary<string, string> map3 = new Dictionary<string, string>();
            map3.Add("zqid", "123456");
            map3.Add("no", "C001");

            string context = GetSignContent(map3);

            string signval = sign(context, private_key);

            Console.Write("签名结果：" + verify(context, signval, public_key, "UTF-8"));
            Console.ReadLine();

        }*/


        public static bool verify(string signContent, string sign, string public_key, string input_charset)
        {
            bool result = false;
            byte[] Data = Encoding.GetEncoding(input_charset).GetBytes(signContent);
            byte[] data = Convert.FromBase64String(sign);
            RSAParameters paraPub = ConvertFromPublicKey(public_key);
            RSACryptoServiceProvider rsaPub = new RSACryptoServiceProvider();
            rsaPub.ImportParameters(paraPub);
            SHA1 sh = new SHA1CryptoServiceProvider();
            result = rsaPub.VerifyData(Data, sh, data);
            return result;
        }


        private static RSAParameters ConvertFromPublicKey(string pemFileConent)
        {

            byte[] keyData = Convert.FromBase64String(pemFileConent);
            if (keyData.Length < 162)
            {
                throw new ArgumentException("pem file content is incorrect.");
            }
            byte[] pemModulus = new byte[128];
            byte[] pemPublicExponent = new byte[3];
            Array.Copy(keyData, 29, pemModulus, 0, 128);
            Array.Copy(keyData, 159, pemPublicExponent, 0, 3);
            RSAParameters para = new RSAParameters();
            para.Modulus = pemModulus;
            para.Exponent = pemPublicExponent;
            return para;
        }


        public static string sign(string content, string privateKey)
        {

            byte[] Data = Encoding.UTF8.GetBytes(content);

            RSACryptoServiceProvider rsa = DecodePemPrivateKey(privateKey);

            SHA1 sh = new SHA1CryptoServiceProvider();

            byte[] signData = rsa.SignData(Data, sh);


            return Convert.ToBase64String(signData);

        }

        
        public static string GetSignContent(IDictionary<string, string> parameters)
        {
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder("");
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key))
                {
                    query.Append(key).Append("=").Append(value).Append("&");
                }
            }
            string content = query.ToString().Substring(0, query.Length - 1);

            return content;

        }

        private static RSACryptoServiceProvider DecodePemPrivateKey(String pemstr)
        {

            byte[] pkcs8privatekey;

            pkcs8privatekey = Convert.FromBase64String(pemstr);

            if (pkcs8privatekey != null)
            {

                RSACryptoServiceProvider rsa = DecodePrivateKeyInfo(pkcs8privatekey);

                return rsa;

            }

            else

                return null;

        }
        private static RSACryptoServiceProvider DecodePrivateKeyInfo(byte[] pkcs8)
        {

            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };

            byte[] seq = new byte[15];

            MemoryStream mem = new MemoryStream(pkcs8);

            int lenstream = (int)mem.Length;

            BinaryReader binr = new BinaryReader(mem); //wrap Memory Stream with BinaryReader for easy reading

            byte bt = 0;

            ushort twobytes = 0;

            try
            {

                twobytes = binr.ReadUInt16();

                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)

                    binr.ReadByte(); //advance 1 byte

                else if (twobytes == 0x8230)

                    binr.ReadInt16(); //advance 2 bytes

                else

                    return null;

                bt = binr.ReadByte();

                if (bt != 0x02)

                    return null;

                twobytes = binr.ReadUInt16();

                if (twobytes != 0x0001)

                    return null;

                seq = binr.ReadBytes(15); //read the Sequence OID

                if (!CompareBytearrays(seq, SeqOID)) //make sure Sequence for OID is correct

                    return null;

                bt = binr.ReadByte();

                if (bt != 0x04) //expect an Octet string

                    return null;

                bt = binr.ReadByte(); //read next byte, or next 2 bytes is 0x81 or 0x82; otherwise bt is the byte count

                if (bt == 0x81)

                    binr.ReadByte();

                else

                    if (bt == 0x82)

                    binr.ReadUInt16();

                //------ at this stage, the remaining sequence should be the RSA private key

                byte[] rsaprivkey = binr.ReadBytes((int)(lenstream - mem.Position));

                RSACryptoServiceProvider rsacsp = DecodeRSAPrivateKey(rsaprivkey);

                return rsacsp;

            }

            catch (Exception)
            {

                return null;

            }

            finally { binr.Close(); }

        }
        private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
        {

            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // --------- Set up stream to decode the asn.1 encoded RSA private key ------

            MemoryStream mem = new MemoryStream(privkey);

            BinaryReader binr = new BinaryReader(mem); //wrap Memory Stream with BinaryReader for easy reading

            byte bt = 0;

            ushort twobytes = 0;

            int elems = 0;

            try
            {

                twobytes = binr.ReadUInt16();

                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)

                    binr.ReadByte(); //advance 1 byte

                else if (twobytes == 0x8230)

                    binr.ReadInt16(); //advance 2 bytes

                else

                    return null;

                twobytes = binr.ReadUInt16();

                if (twobytes != 0x0102) //version number

                    return null;

                bt = binr.ReadByte();

                if (bt != 0x00)

                    return null;

                //------ all private key components are Integer sequences ----

                elems = GetIntegerSize(binr);

                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);

                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);

                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);

                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);

                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);

                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);

                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);

                IQ = binr.ReadBytes(elems);

                // ------- create RSACryptoServiceProvider instance and initialize with public key -----

                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

                RSAParameters RSAparams = new RSAParameters();

                RSAparams.Modulus = MODULUS;

                RSAparams.Exponent = E;

                RSAparams.D = D;

                RSAparams.P = P;

                RSAparams.Q = Q;

                RSAparams.DP = DP;

                RSAparams.DQ = DQ;

                RSAparams.InverseQ = IQ;

                RSA.ImportParameters(RSAparams);

                return RSA;

            }

            catch (Exception)
            {

                return null;

            }

            finally { binr.Close(); }

        }
        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)		//expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();	// data size in next byte
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte(); // data size in next 2 bytes
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;     // we already have the data size
            }

            while (binr.ReadByte() == 0x00)
            {	//remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);		//last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }
        private static bool CompareBytearrays(byte[] a, byte[] b)
        {

            if (a.Length != b.Length)

                return false;

            int i = 0;

            foreach (byte c in a)
            {

                if (c != b[i])

                    return false;

                i++;

            }

            return true;

        }
    }

}
