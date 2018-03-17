using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using NinjaDomain.Classes;
using NinjaDomain.DataModel;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new NullDatabaseInitializer<NinjaContext>());
            //InsertNinja();
            //InsertMultipleNinjas(); //LD "AddRange" can insert a list of items at the same time
            //SimpleNinjaQueries();
            //QueryAndUpdateNinja();
            //DeleteNinja();
            //RetrieveDataWithFind();
            //RetrieveDataWithStoredProc();
            //DeleteNinjaWithKeyValue();
            //DeleteNinjaViaStoredProcedure();
            //QueryAndUpdateNinjaDisconnected();

            InsertNinjaWithEquipment();
            //SimpleNinjaGraphQuery();
            //ProjectionQuery();
            //QueryAndUpdateNinjaDisconnected();

            //ReseedDatabase();
            Console.ReadKey();
        }



        private static void InsertNinja()
        {
            var ninja = new Ninja
            {
                Name = "SampsonSan",
                ServedInOniwaban = false,
                //DateOfBirth = new DateTime(2008, 1, 28),
                ClanId = 1

            };
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;//LD useful to see what's app in the database
                context.Ninjas.Add(ninja);
                context.SaveChanges();
            }
        }

        private static void InsertMultipleNinjas()
        {
            var ninja1 = new Ninja
            {
                Name = "Leonardo",
                ServedInOniwaban = false,
                DateOfBirth = new DateTime(1984, 1, 1),
                ClanId = 1
            };
            var ninja2 = new Ninja
            {
                Name = "Raphael",
                ServedInOniwaban = false,
                DateOfBirth = new DateTime(1985, 1, 1),
                ClanId = 1
            };
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.AddRange(new List<Ninja> { ninja1, ninja2 });
                context.SaveChanges();
            }
        }

        private static void SimpleNinjaQueries()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                //LD way 1 to do
                //var query = context.Ninjas; //LD I can use the definition of it in a list or in an enumerable(FOR)
                //var someninjas = query.ToList();

                //LD way 2 to do
                var ninjas = context.Ninjas
                    .Where(n => n.DateOfBirth >= new DateTime(1984, 1, 1))
                    .OrderBy(n => n.Name)
                    .Skip(1).Take(1);

                foreach (var ninja in ninjas)
                {
                    Console.WriteLine(ninja.Name);
                }
            }
        }

        private static void QueryAndUpdateNinja()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.FirstOrDefault();
                ninja.ServedInOniwaban = (!ninja.ServedInOniwaban);
                context.SaveChanges();
            }
        }

        private static void QueryAndUpdateNinjaDisconnected()
        {
            Ninja ninja;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                ninja = context.Ninjas.FirstOrDefault();
            }

            ninja.Name  = "testicolo";

            //LD note that when we change context, EF don't know nothing about the ninja object, so it's not enough do the "ACTION ONE"
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                
                //LD ABOBE THE TRICK OF THE UPDATE
                //context.Ninjas .Add(ninja); //LD if we just use this, a new record will be added and then not updated
                context.Ninjas.Attach(ninja);//LD until now we tell to EF: Look you have to deal with this entity of ninja
                context.Entry(ninja).State = EntityState.Modified; //LD now the context know that the object should be updated in the database. 

                context.SaveChanges(); //LD ACTION ONE
            }
        }

        private static void RetrieveDataWithFind()
        {
            var keyval = 2;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.Find(keyval);
                Console.WriteLine("After Find#1:" + ninja.Name);

                //LD here we don't query for a second time the database because we have already everithyng in memory
                var someNinja = context.Ninjas.Find(keyval);
                Console.WriteLine("After Find#2:" + someNinja.Name);
                ninja = null;
            }
        }

        //private static void RetrieveDataWithStoredProc()
        //{

        //    using (var context = new NinjaContext())
        //    {
        //        context.Database.Log = Console.WriteLine;
        //        var ninjas = context.Ninjas.SqlQuery("exec GetOldNinjas").ToList();
        //        //foreach (var ninja in ninjas)
        //        //{
        //        //    Console.WriteLine(ninja.Name);
        //        //}
        //    }
        //}

            //LD useful in disconnected scenarios
        private static void DeleteNinja()
        {
            Ninja ninja;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                ninja = context.Ninjas.FirstOrDefault(); //LD trip one to the db
                //context.Ninjas.Remove(ninja);
                //context.SaveChanges();
            }
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                //context.Ninjas.Attach(ninja); //LD the Attach is done in default after that below we set "EntityState.Deleted" 
                //context.Ninjas.Remove(ninja);
                context.Entry(ninja).State = EntityState.Deleted;
                context.SaveChanges(); //LD trip two to the db
            }
        }

        //LD the approach below is simple but we have to do two trip to the db
        private static void DeleteNinjaWithKeyValue()
        {
            var keyval = 1;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.Find(keyval); //LD trip one
                context.Ninjas.Remove(ninja);
                context.SaveChanges(); //LD trip two
            }
        }

        //private static void DeleteNinjaViaStoredProcedure()
        //{
        //    var keyval = 3;
        //    using (var context = new NinjaContext())
        //    {
        //        context.Database.Log = Console.WriteLine;
        //        context.Database.ExecuteSqlCommand(
        //            "exec DeleteNinjaViaId {0}", keyval);
        //    }
        //}

        private static void InsertNinjaWithEquipment()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                var ninja = new Ninja
                {
                    Name = "Kacy Catanzaro",
                    ServedInOniwaban = false,
                    DateOfBirth = new DateTime(1990, 1, 14),
                    ClanId = 1
                };
                var muscles = new NinjaEquipment
                {
                    Name = "Muscles",
                    Type = EquipmentType.Tool,

                };
                var spunk = new NinjaEquipment
                {
                    Name = "Spunk",
                    Type = EquipmentType.Weapon
                };

         //LD here we add some object "NinjaEquipment" to the "EquipmentOwned" of this specific ninja
        //LD TO DON'T HAVE ERRORS HERE I HAVE TO INITIALIZE "EquipmentOwned" in the class ninja
        //public Ninja()
        //{
        //    EquipmentOwned = new List<NinjaEquipment>();
        //}
                ninja.EquipmentOwned.Add(muscles);
                ninja.EquipmentOwned.Add(spunk);

                context.Ninjas.Add(ninja);
                context.SaveChanges();
            }

        }

        private static void SimpleNinjaGraphQuery()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                //var ninjas = context.Ninjas.Include(n => n.EquipmentOwned)
                //    .FirstOrDefault(n => n.Name.StartsWith("Kacy"));

                var ninja = context.Ninjas
                           .FirstOrDefault(n => n.Name.StartsWith("Kacy"));
                Console.WriteLine("Ninja Retrieved:" + ninja.Name);
                context.Entry(ninja).Collection(n => n.EquipmentOwned).Load();
            }

        }

        //private static void ProjectionQuery()
        //{
        //    using (var context = new NinjaContext())
        //    {
        //        context.Database.Log = Console.WriteLine;
        //        var ninjas = context.Ninjas
        //            .Select(n => new { n.Name, n.DateOfBirth, n.EquipmentOwned })
        //            .ToList();

        //    }
        //}

        //private static void ReseedDatabase()
        //{
        //    Database.SetInitializer(new DropCreateDatabaseAlways<NinjaContext>());
        //    using (var context = new NinjaContext())
        //    {
        //        context.Clans.Add(new Clan { ClanName = "Vermont Clan" });
        //        var j = new Ninja
        //        {
        //            Name = "JulieSan",
        //            ServedInOniwaban = false,
        //            DateOfBirth = new DateTime(1980, 1, 1),
        //            ClanId = 1

        //        };
        //        var s = new Ninja
        //        {
        //            Name = "SampsonSan",
        //            ServedInOniwaban = false,
        //            DateOfBirth = new DateTime(2008, 1, 28),
        //            ClanId = 1

        //        };
        //        var l = new Ninja
        //        {
        //            Name = "Leonardo",
        //            ServedInOniwaban = false,
        //            DateOfBirth = new DateTime(1984, 1, 1),
        //            ClanId = 1
        //        };
        //        var r = new Ninja
        //        {
        //            Name = "Raphael",
        //            ServedInOniwaban = false,
        //            DateOfBirth = new DateTime(1985, 1, 1),
        //            ClanId = 1
        //        };
        //        context.Ninjas.AddRange(new List<Ninja> { j, s, l, r });
        //        context.SaveChanges();
        //        context.Database.ExecuteSqlCommand(
        //          @"CREATE PROCEDURE GetOldNinjas
        //            AS  SELECT * FROM Ninjas WHERE DateOfBirth<='1/1/1980'");

        //        context.Database.ExecuteSqlCommand(
        //           @"CREATE PROCEDURE DeleteNinjaViaId
        //             @Id int
        //             AS
        //             DELETE from Ninjas Where Id = @id
        //             RETURN @@rowcount");
        //    }

        //}
    }
}