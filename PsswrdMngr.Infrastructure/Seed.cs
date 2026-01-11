using PsswrdMngr.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PsswrdMngr.Infrastructure
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<User>
                {
                    new User
                    {
                        UserName = "TestUser",
                        Email = "test@test.com",
                        // Bcrypt hash generated with https://bcrypt.online/ tool using cost factor 10 for password "testPassword"
                        Password = "$2y$10$5BV/tn9bz6VmLJ8VDHqZduncFjNL7363.2Fdkc.d3qi2qXT6U7..i",                         
                        // RSA public key of length 1024 with https://cryptotools.net/rsagen 
                        // RSA private key to go with it: MIICWwIBAAKBgG87C72B1cmqa7lpqeJ4Lad3vrcF04CeWsgndHZi3Ryo+V2oxzPAkbtNIQXhEWyU2CJkcd21KS5wJR0OGpp7P/dOMV4WzE9VrNbh3FVyJeN5ACXA8WWw6K/D3622sa1aUdCZXs3Qzr6bUMXuJvL47PEeO4Xch14/ygUEPhrNnqgpAgMBAAECgYBBjar9pOc6UxXp0DwvHGTLrebYNrbPtoQKMjaRDvMBURSl/jJobbV1jZ9It7xtIcu/eTMiVwJOPAmjdgx3vuuTKgOCbD1HRCBoSoJzLgYlfbjBqVYd3/vg+GmPsTlXCTb/Eepmh09mCQCnSF/oFy59YdOqOwGfNfW8vGZf4M6wAQJBAMytZ/cY+JPGlh//lTcIjcUsPh/d9cbqC5uun0l3v65Hijq7XzlrxOABORVyvJjndp2lsDL3Sns3zTedI2DunHkCQQCLHyIiq/+TnJLEH8ngzk5A4h6MWFIS+HvvtEpFt28+7gLdMm9Shau7DeeQVN9DPR+d9CMxOcrc0siKbyYJwR0xAkEAiGDy6T6GQ1ELLv8J2cWlUUANlQY8gfHpEi7ki/tyiRm6DTBUXtpkMC4gbx4odGmmK5z/LrZ/tmKUJv5ZWHklYQJAWvY2OXlAF+GNzJIESupZG+88jgLJVwIE9storICtSfYRBVOOmeQwTpNezm3PtITogTw5LlxdGVjwKiNWOZZ8YQJAP1W490X5xU4A5+dzKJ88YqqoBVf3Kbo7B7iBAJyQbCI1BspmdckrqB9ZRgpPsBDE6ZqpVA7q9NHMxrb8ACp0NA==
                        PublicKey = "MIGeMA0GCSqGSIb3DQEBAQUAA4GMADCBiAKBgG87C72B1cmqa7lpqeJ4Lad3vrcF04CeWsgndHZi3Ryo+V2oxzPAkbtNIQXhEWyU2CJkcd21KS5wJR0OGpp7P/dOMV4WzE9VrNbh3FVyJeN5ACXA8WWw6K/D3622sa1aUdCZXs3Qzr6bUMXuJvL47PEeO4Xch14/ygUEPhrNnqgpAgMBAAE=",
                    },
                    new User
                    {
                        UserName = "Overlord",
                        Email = "overlord@overlord.net",
                        // Bcrypt hash generated with https://bcrypt.online/ tool using cost factor 10 for password "Zaq12wsx"
                        Password = "$2y$10$Q21U.M8MqGO3IDZ3hq6/wOiSJJqBKGFKAXViayq4AknyHLEkTxVcS",
                        // RSA public key of length 1024 with https://cryptotools.net/rsagen 
                        // RSA private key to go with it: MIICWwIBAAKBgQCno7nbfzDo0jY9iWtBD+BXTkNteCQ1NEq2VBYDNS2KzgPsGHz25M2d4xqfsI1fBImIm5V5ohswjYfu0QeRD5pnKzWL6DgkmeWA2O4cfqu2NQAk3ffLS2Hr88NYgspvgHyVigOndk2J0tRyLZOnrPIF299IVTEQdT+XdDOVXzL5yQIDAQABAoGABp1lRg38xhMVZNQ5UTXpKkjCTF2DQD4x/IPQ4ouEooeCjBxjQfLBUkuVz1tOGMO7EZLiEQyeegn7pSbGX6j39K2aYRySly0tAiPwFjFwazoTPGVuWtu7abHz0P3WeWCPh9Jmj4z2twGNtrLJ/83AiN7g/dv0w4yFP2Lz88IuSQkCQQD3ZyNvXZdulifT7oQ6415UlmjqZmlblMGtOjSrZs49kadvXYg3ovZzRf22Fx0UFiXrY8vDgGCHduN8/fZT9Vb3AkEArXcGLNr45f7Vdgca/AzIFmsSzXrIfIt9S7tV94QtiIUgM8FzCpGhzMZ2ADpatVX48a8WtXbsYuosIA+Jcy5FPwJAeG4uY6Gte1mAnbu3hmrzmj11aNTdaIUrGkYBKYZr0rC6To27J0oeqdJiRGdP8l0trD3yDILLemW3Kzr807XT1QJAMCdXdoI/EBHjDgXA7vFZZifJK3OHTlOmr6xMnA58WWajXtq35doxsVfyj/OjFK3OEsGJK0zdKERbhXbqsCfLHwJAcpP9HFnVNoUAFGNvmWsAp7qh14pwXUdfEuJxwuO0pEiaW85SGvgO8lgIJq2/wdkkTTWiXlyyZk2CJHnUEPavGw==
                        PublicKey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCno7nbfzDo0jY9iWtBD+BXTkNteCQ1NEq2VBYDNS2KzgPsGHz25M2d4xqfsI1fBImIm5V5ohswjYfu0QeRD5pnKzWL6DgkmeWA2O4cfqu2NQAk3ffLS2Hr88NYgspvgHyVigOndk2J0tRyLZOnrPIF299IVTEQdT+XdDOVXzL5yQIDAQAB",
                    }
                };
                
                foreach (var user in users)
                {
                    Console.WriteLine(user.Id);
                    var res = await userManager.CreateAsync(user, user.Password);
                    if (!res.Succeeded)
                    {
                        foreach (var err in res.Errors)
                        {
                            Console.WriteLine(err);
                            Console.WriteLine(err.Description);
                        }
                    }
                }
            }

            if (!context.CredentialContainers.Any())
            {
                List<User> users = await context.Users.ToListAsync();
                Guid uid = Guid.Parse(users[0].Id);

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
