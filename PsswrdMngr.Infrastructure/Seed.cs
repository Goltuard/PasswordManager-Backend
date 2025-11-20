using PsswrdMngr.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace PsswrdMngr.Infrastructure
{
    public class Seed
    {
        public static async Task SeedData(DataContext context)
        {
            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new User
                    {
                        Name = "TestUser",
                        // Bcrypt hash generated with https://bcrypt.online/ tool using cost factor 10 for password "testPassword"
                        PasswordHash = "$2y$10$5BV/tn9bz6VmLJ8VDHqZduncFjNL7363.2Fdkc.d3qi2qXT6U7..i",                         
                        // RSA public key of length 1024 with https://cryptotools.net/rsagen 
                        // RSA private key to go with it: MIICWwIBAAKBgG87C72B1cmqa7lpqeJ4Lad3vrcF04CeWsgndHZi3Ryo+V2oxzPAkbtNIQXhEWyU2CJkcd21KS5wJR0OGpp7P/dOMV4WzE9VrNbh3FVyJeN5ACXA8WWw6K/D3622sa1aUdCZXs3Qzr6bUMXuJvL47PEeO4Xch14/ygUEPhrNnqgpAgMBAAECgYBBjar9pOc6UxXp0DwvHGTLrebYNrbPtoQKMjaRDvMBURSl/jJobbV1jZ9It7xtIcu/eTMiVwJOPAmjdgx3vuuTKgOCbD1HRCBoSoJzLgYlfbjBqVYd3/vg+GmPsTlXCTb/Eepmh09mCQCnSF/oFy59YdOqOwGfNfW8vGZf4M6wAQJBAMytZ/cY+JPGlh//lTcIjcUsPh/d9cbqC5uun0l3v65Hijq7XzlrxOABORVyvJjndp2lsDL3Sns3zTedI2DunHkCQQCLHyIiq/+TnJLEH8ngzk5A4h6MWFIS+HvvtEpFt28+7gLdMm9Shau7DeeQVN9DPR+d9CMxOcrc0siKbyYJwR0xAkEAiGDy6T6GQ1ELLv8J2cWlUUANlQY8gfHpEi7ki/tyiRm6DTBUXtpkMC4gbx4odGmmK5z/LrZ/tmKUJv5ZWHklYQJAWvY2OXlAF+GNzJIESupZG+88jgLJVwIE9storICtSfYRBVOOmeQwTpNezm3PtITogTw5LlxdGVjwKiNWOZZ8YQJAP1W490X5xU4A5+dzKJ88YqqoBVf3Kbo7B7iBAJyQbCI1BspmdckrqB9ZRgpPsBDE6ZqpVA7q9NHMxrb8ACp0NA==
                        PublicKey = "MIGeMA0GCSqGSIb3DQEBAQUAA4GMADCBiAKBgG87C72B1cmqa7lpqeJ4Lad3vrcF04CeWsgndHZi3Ryo+V2oxzPAkbtNIQXhEWyU2CJkcd21KS5wJR0OGpp7P/dOMV4WzE9VrNbh3FVyJeN5ACXA8WWw6K/D3622sa1aUdCZXs3Qzr6bUMXuJvL47PEeO4Xch14/ygUEPhrNnqgpAgMBAAE="
                    }
                };
                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();
            }

            if (!context.CredentialContainers.Any())
            {
                List<User> users = await context.Users.ToListAsync();
                Guid uid = users[0].Id;

                var containers = new List<CredentialContainer>
                {
                    new CredentialContainer
                    {
                        UserId = uid,
                        // ContainerString is currently unencrypted for test purposes
                        ContainerString =
                            "{\"serviceName\":\"example.com\",\"userName\":\"exampleUser\",\"password\":\"examplePassword\",\"note\":\"This is example content of a credential container\"}",
                        // SHA3-512 hash of the above test string. Generated with https://codebeautify.org/sha3-512-hash-generator
                        ContainerHash =
                            "f10bb17fc840e7569990e7b69bd9a9e026bbb5c61c34285fb024619fcf9438c59708ac606b41f0c621db18a40a3343167729b891bdf9122172c5230bca8c747a"
                    }
                };
                await context.CredentialContainers.AddRangeAsync(containers);
                await context.SaveChangesAsync();
            }
        }
    }
}
