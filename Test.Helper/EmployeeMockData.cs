using Models;

namespace Test.Helper
{
    public class EmployeeMockData
    {
        public static IEnumerable<Employee> GetEmployees()
        {
            var employees = new List<Employee>
            {
               new Employee(){Id=1,FirstName="Debra",LastName="Burks",EmailAddress="debra.burks@yahoo.com",Addresses=new List<Address>{new Address(){Id= 1,City="Orchard Park",State="NY",Street="9273 Thorne Ave. ",ZipCode="14127"}}},
               new Employee(){Id=2,FirstName="Kasha",LastName="Todd",EmailAddress="kasha.todd@yahoo.com",Addresses=new List<Address>{new Address(){Id= 2,City="Campbell",State="CA",Street="910 Vine Street ",ZipCode="95008"}}},
               new Employee(){Id=3,FirstName="Tameka",LastName="Fisher",EmailAddress="tameka.fisher@aol.com",Addresses=new List<Address>{new Address(){Id= 3,City="Redondo Beach",State="CA",Street="769C Honey Creek St. ",ZipCode="90278"}}},
               new Employee(){Id=4,FirstName="Daryl",LastName="Spence",EmailAddress="daryl.spence@aol.com",Addresses=new List<Address>{new Address(){Id= 4,City="Uniondale",State="NY",Street="988 Pearl Lane ",ZipCode="11553"}}},
               new Employee(){Id=5,FirstName="Charolette",LastName="Rice",EmailAddress="charolette.rice@msn.com",Addresses=new List<Address>{new Address(){Id= 5,City="Sacramento",State="CA",Street="107 River Dr. ",ZipCode="95820"}}},
               new Employee(){Id=6,FirstName="Lyndsey",LastName="Bean",EmailAddress="lyndsey.bean@hotmail.com",Addresses=new List<Address>{new Address(){Id= 6,City="Fairport",State="NY",Street="769 West Road ",ZipCode="14450"}}},
               new Employee(){Id=7,FirstName="Latasha",LastName="Hays",EmailAddress="latasha.hays@hotmail.com",Addresses=new List<Address>{new Address(){Id= 7,City="Buffalo",State="NY",Street="7014 Manor Station Rd. ",ZipCode="14215"}}},
               new Employee(){Id=8,FirstName="Jacquline",LastName="Duncan",EmailAddress="jacquline.duncan@yahoo.com",Addresses=new List<Address>{new Address(){Id= 8,City="Jackson Heights",State="NY",Street="15 Brown St. ",ZipCode="11372"}}},
               new Employee(){Id=9,FirstName="Genoveva",LastName="Baldwin",EmailAddress="genoveva.baldwin@msn.com",Addresses=new List<Address>{new Address(){Id= 9,City="Port Washington",State="NY",Street="8550 Spruce Drive ",ZipCode="11050"}}},
               new Employee(){Id=10,FirstName="Pamelia",LastName="Newman",EmailAddress="pamelia.newman@gmail.com",Addresses=new List<Address>{new Address(){Id= 10,City="Monroe",State="NY",Street="476 Chestnut Ave. ",ZipCode="10950"}}},
               new Employee(){Id=11,FirstName="Deshawn",LastName="Mendoza",EmailAddress="deshawn.mendoza@yahoo.com",Addresses=new List<Address>{new Address(){Id= 11,City="Monsey",State="NY",Street="8790 Cobblestone Street ",ZipCode="10952"}}},
               new Employee(){Id=12,FirstName="Robby",LastName="Sykes",EmailAddress="robby.sykes@hotmail.com",Addresses=new List<Address>{new Address(){Id= 12,City="Hempstead",State="NY",Street="486 Rock Maple Street ",ZipCode="11550"}}},
               new Employee(){Id=13,FirstName="Lashawn",LastName="Ortiz",EmailAddress="lashawn.ortiz@msn.com",Addresses=new List<Address>{new Address(){Id= 13,City="Longview",State="TX",Street="27 Washington Rd. ",ZipCode="75604"}}},
               new Employee(){Id=14,FirstName="Garry",LastName="Espinoza",EmailAddress="garry.espinoza@hotmail.com",Addresses=new List<Address>{new Address(){Id= 14,City="Forney",State="TX",Street="7858 Rockaway Court ",ZipCode="75126"}}},
               new Employee(){Id=15,FirstName="Linnie",LastName="Branch",EmailAddress="linnie.branch@gmail.com",Addresses=new List<Address>{new Address(){Id= 15,City="Plattsburgh",State="NY",Street="314 South Columbia Ave. ",ZipCode="12901"}}},
               new Employee(){Id=16,FirstName="Emmitt",LastName="Sanchez",EmailAddress="emmitt.sanchez@hotmail.com",Addresses=new List<Address>{new Address(){Id= 16,City="New York",State="NY",Street="461 Squaw Creek Road ",ZipCode="10002"}}},
               new Employee(){Id=17,FirstName="Caren",LastName="Stephens",EmailAddress="caren.stephens@msn.com",Addresses=new List<Address>{new Address(){Id= 17,City="Scarsdale",State="NY",Street="914 Brook St. ",ZipCode="10583"}}},
               new Employee(){Id=18,FirstName="Georgetta",LastName="Hardin",EmailAddress="georgetta.hardin@aol.com",Addresses=new List<Address>{new Address(){Id= 18,City="Canandaigua",State="NY",Street="474 Chapel Dr. ",ZipCode="14424"}}},
               new Employee(){Id=19,FirstName="Lizzette",LastName="Stein",EmailAddress="lizzette.stein@yahoo.com",Addresses=new List<Address>{new Address(){Id= 19,City="Orchard Park",State="NY",Street="19 Green Hill Lane ",ZipCode="14127"}}},
               new Employee(){Id=20,FirstName="Aleta",LastName="Shepard",EmailAddress="aleta.shepard@aol.com",Addresses=new List<Address>{new Address(){Id= 20,City="Sugar Land",State="TX",Street="684 Howard St. ",ZipCode="77478"}}}
            };

            return employees;
        }
    }
}