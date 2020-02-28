using bFit.Web.Helpers;
using bFit.WEB.Data.Entities;
using bFit.WEB.Data.Entities.Common;
using bFit.WEB.Data.Entities.PersonalData;
using bFit.WEB.Data.Entities.Profiles;
using bFit.WEB.Data.Entities.Workouts;
using System;
using System.Linq;
using System.Threading.Tasks;
#nullable enable

namespace bFit.Web.Data
{
    public class DbSeeder
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IUserHelper _userHelper;

        public DbSeeder(ApplicationDbContext applicationDbContext, IUserHelper userHelper)
        {
            _applicationDbContext = applicationDbContext;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _applicationDbContext.Database.EnsureCreatedAsync();

            await CheckCountriesAsync();
            await CheckStatesAsync();
            await CheckCountiesAsync();
            await CheckDistrictsAsync();
            await CheckTownsAsync();
            await CheckGendersAsync();
            await CheckObesityLevelAsync();
            await CheckFranchisesAsync();
            await CheckGymsAsync();

            await CheckExerciseTypesAsync();
            await CheckExercisesAsync();
            await CheckGoalsAsync();

            await CheckRoles();

            var admin = await CheckUserAsync("701570777", "Alonso", "Ugalde", "Aguilar",
                "augaldecr@gmail.com", "85090266", "Coopevigua 2", "Admin");
            var customer = await CheckUserAsync("701570777", "Alonso", "Ugalde", "Aguilar",
                "augaldecr@hotmail.com", "85090266", "Coopevigua 2", "Customer");
            var trainer = await CheckUserAsync("701570888", "Alonso", "Ugalde", "Aguilar",
                "alonsougaldecr@gmail.com", "85090266", "Coopevigua 2", "Trainer");

            await CheckAdminAsync(admin);
            await CheckCustomerAsync(customer);
            await CheckTrainerAsync(trainer);

            await CheckSetTypesAsync();
            await CheckWorkoutsAsync();
            await CheckSubSetsAsync();

            await CheckDataTakeAsync();
        }

        private async Task CheckWorkoutsAsync()
        {
            if (!_applicationDbContext.WorkoutRoutines.Any())
            {
                var customer = _applicationDbContext.Customers.FirstOrDefault();
                var goal = _applicationDbContext.Goals.FirstOrDefault();
                var trainer = _applicationDbContext.Trainers.FirstOrDefault();

                await _applicationDbContext.WorkoutRoutines.AddAsync(
                    new WorkoutRoutine
                    {
                        Customer = customer,
                        Begins = new DateTime(2020, 03, 1),
                        Ends = new DateTime(2020, 03, 31),
                        Goal = goal,
                        Trainer = trainer
                    });
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        private async Task CheckSubSetsAsync()
        {
            if (!_applicationDbContext.SubSets.Any())
            {
                var setType = _applicationDbContext.SetTypes.FirstOrDefault();
                var workOutRoutine = _applicationDbContext.WorkoutRoutines.FirstOrDefault();
                var customerId = workOutRoutine.Customer.Id;

                var set1 = await CheckSetsAsync($"{customerId}{DateTime.Now}",
                    setType, workOutRoutine);
                var set2 = await CheckSetsAsync($"{customerId}{DateTime.Now}",
                    setType, workOutRoutine);
                var set3 = await CheckSetsAsync($"{customerId}{DateTime.Now}",
                    setType, workOutRoutine);

                var exercise1 = _applicationDbContext.Exercises.FirstOrDefault(
                    e => e.Name == "Sentadilla libre");
                var exercise2 = _applicationDbContext.Exercises.FirstOrDefault(
                    e => e.Name.Equals("Sentadilla máquina"));
                var exercise3 = _applicationDbContext.Exercises.FirstOrDefault(
                    e => e.Name.Equals("Desplante"));
                var exercise4 = _applicationDbContext.Exercises.FirstOrDefault(
                    e => e.Name.Equals("Press banca"));
                var exercise5 = _applicationDbContext.Exercises.FirstOrDefault(
                    e => e.Name.Equals("Press mancuerna"));
                var exercise6 = _applicationDbContext.Exercises.FirstOrDefault(
                    e => e.Name.Equals("Curl barra"));

                await _applicationDbContext.SubSets.AddAsync(
                    new SubSet
                    {
                        Exercise = exercise1,
                        PositiveTime = 1,
                        NegativeTime = 3,
                        Quantity = 8,
                        Set = set1
                    });
                await _applicationDbContext.SubSets.AddAsync(
                    new SubSet
                    {
                        Exercise = exercise2,
                        PositiveTime = 1,
                        NegativeTime = 3,
                        Quantity = 10,
                        Set = set1
                    });
                await _applicationDbContext.SubSets.AddAsync(
                    new SubSet
                    {
                        Exercise = exercise3,
                        PositiveTime = 1,
                        NegativeTime = 3,
                        Quantity = 12,
                        Set = set2
                    });
                await _applicationDbContext.SubSets.AddAsync(
                    new SubSet
                    {
                        Exercise = exercise4,
                        PositiveTime = 1,
                        NegativeTime = 3,
                        Quantity = 12,
                        Set = set2
                    });
                await _applicationDbContext.SubSets.AddAsync(
                    new SubSet
                    {
                        Exercise = exercise5,
                        PositiveTime = 1,
                        NegativeTime = 3,
                        Quantity = 12,
                        Set = set3
                    });
                await _applicationDbContext.SubSets.AddAsync(
                    new SubSet
                    {
                        Exercise = exercise6,
                        PositiveTime = 1,
                        NegativeTime = 3,
                        Quantity = 12,
                        Set = set3
                    });
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        private async Task<Set> CheckSetsAsync(string name, SetType setType, WorkoutRoutine workoutRoutine)
        {
            var set = _applicationDbContext.Sets.FirstOrDefault(s => s.Name.Equals(name));

            if (set == null)
            {
                set = new Set
                {
                    Name = name,
                    SetType = setType,
                    WorkoutRoutine = workoutRoutine
                };

                await _applicationDbContext.Sets.AddAsync(set);
                await _applicationDbContext.SaveChangesAsync();
                return _applicationDbContext.Sets.FirstOrDefault(s => s.Name.Equals(name));
            }

            return set;
        }

        private async Task CheckSetTypesAsync()
        {
            if (!_applicationDbContext.SetTypes.Any())
            {
                await _applicationDbContext.SetTypes.AddAsync(new SetType { Name = "Escalera" });
                await _applicationDbContext.SetTypes.AddAsync(new SetType { Name = "Escalera invertida" });
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        private async Task CheckGoalsAsync()
        {
            if (!_applicationDbContext.Goals.Any())
            {
                await _applicationDbContext.Goals.AddAsync(new Goal { Name = "Condición física" });
                await _applicationDbContext.Goals.AddAsync(new Goal { Name = "Hipertrofia" });
                await _applicationDbContext.Goals.AddAsync(new Goal { Name = "Quema de grasa" });
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        private async Task CheckExercisesAsync()
        {
            if (!_applicationDbContext.Exercises.Any())
            {
                var exerciseType = _applicationDbContext.ExerciseTypes.FirstOrDefault();

                await _applicationDbContext.Exercises.AddAsync(
                    new Exercise { Name = "Sentadilla libre", ExerciseType = exerciseType });
                await _applicationDbContext.Exercises.AddAsync(
                    new Exercise { Name = "Sentadilla máquina", ExerciseType = exerciseType });
                await _applicationDbContext.Exercises.AddAsync(
                    new Exercise { Name = "Desplante", ExerciseType = exerciseType });
                await _applicationDbContext.Exercises.AddAsync(
                    new Exercise { Name = "Press banca", ExerciseType = exerciseType });
                await _applicationDbContext.Exercises.AddAsync(
                    new Exercise { Name = "Press mancuerna", ExerciseType = exerciseType });
                await _applicationDbContext.Exercises.AddAsync(
                    new Exercise { Name = "Curl barra", ExerciseType = exerciseType });
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        private async Task CheckExerciseTypesAsync()
        {
            if (!_applicationDbContext.ExerciseTypes.Any())
            {
                await _applicationDbContext.ExerciseTypes.AddAsync(new ExerciseType { Name = "Aeróbico" });
                await _applicationDbContext.ExerciseTypes.AddAsync(new ExerciseType { Name = "Anaeróbico" });
                await _applicationDbContext.ExerciseTypes.AddAsync(new ExerciseType { Name = "Flexibilidad" });
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        private async Task CheckObesityLevelAsync()
        {
            if (!_applicationDbContext.ObesityLevels.Any())
            {
                await _applicationDbContext.ObesityLevels.AddAsync(new ObesityLevel { Name = "Normal" });
                await _applicationDbContext.ObesityLevels.AddAsync(new ObesityLevel { Name = "Sobrepeso" });
                await _applicationDbContext.ObesityLevels.AddAsync(new ObesityLevel { Name = "Obesidad 1" });
                await _applicationDbContext.ObesityLevels.AddAsync(new ObesityLevel { Name = "Obesidad 2" });
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        private async Task CheckGymsAsync()
        {
            if (!_applicationDbContext.Gyms.Any())
            {
                var franchise = _applicationDbContext.Franchises.FirstOrDefault(f => f.LegalId.Equals("1111"));
                var town = _applicationDbContext.Towns.FirstOrDefault(t => t.Name.Equals("Guápiles"));

                await _applicationDbContext.Gyms.AddAsync(new LocalGym
                {
                    Email = "contacto@irontraining.com",
                    PhoneNumber = "22222222",
                    Address = "Guápiles centro",
                    Franchise = franchise,
                    Town = town
                });
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        private async Task CheckTrainerAsync(User user)
        {
            if (!_applicationDbContext.Trainers.Any())
            {
                //var gym = _applicationDbContext.Gyms.FirstOrDefault(t => t.Email.Equals("contacto@irontraining.com"));
                var franchise = _applicationDbContext.Franchises.FirstOrDefault(
                    t => t.Email.Equals("irontraining@gmail.com"));

                await _applicationDbContext.Trainers.AddAsync(new Trainer
                {
                    User = user,
                    Franchise = franchise
                });
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        private async Task CheckDataTakeAsync()
        {
            if (!_applicationDbContext.PersonalData.Any())
            {
                var trainer = _applicationDbContext.Trainers.FirstOrDefault();
                var obesityLevel = _applicationDbContext.ObesityLevels.FirstOrDefault();

                await _applicationDbContext.PersonalData.AddAsync(new DataTake
                {
                    Trainer = trainer,
                    Date = new DateTime(2020, 03, 01),
                    Height = 1.85,
                    Weight = 95,
                    MuscleEsquelethicalMassKG = 50,
                    FatMassKG = 15,
                    WaterMass = 30,
                    MetabolicAge = 65,
                    ChestBack = 100,
                    Waist = 100,
                    Abdomen = 90,
                    Hip = 90,
                    LeftArm = 35,
                    RightArm = 35,
                    LeftQuadriceps = 35,
                    RightQuadriceps = 35,
                    LeftCalf = 35,
                    RightCalf = 35,
                    LeftForearm = 35,
                    RightForearm = 35,
                    VisceralFatLevel = 5,
                    BasalMetabolicRate = 1500,
                    RecommendedCaloricIntake = 1450,
                    ObesityLevel = obesityLevel
                });
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        private async Task CheckFranchisesAsync()
        {
            if (!_applicationDbContext.Franchises.Any())
            {
                await _applicationDbContext.Franchises.AddAsync(new Franchise
                {
                    LegalId = "1111",
                    TradeName = "Iron Fit Training",
                    RegisteredName = "Iron Traing",
                    Email = "irontraining@gmail.com",
                    PhoneNumber = "88888888"
                });
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        private async Task CheckRoles()
        {
            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("FranchiseAdmin");
            await _userHelper.CheckRoleAsync("GymAdmin");
            await _userHelper.CheckRoleAsync("Trainer");
            await _userHelper.CheckRoleAsync("Customer");
        }

        private async Task<User> CheckUserAsync(string socialSecurityId, string firstName,
            string lastName1, string lastName2, string email, string phone, string address, string role)
        {
            var user = await _userHelper.GetUserByEmailAsync(email);

            if (user == null)
            {
                var town = _applicationDbContext.Towns.FirstOrDefault(t => t.Name.Equals("Guápiles"));

                user = new User
                {
                    SocialSecurityId = socialSecurityId,
                    FirstName = firstName,
                    LastName1 = lastName1,
                    LastName2 = lastName2,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Town = town
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, role);
            }

            return user;
        }

        private async Task CheckCustomerAsync(User user)
        {
            if (!_applicationDbContext.Customers.Any())
            {
                var masculino = _applicationDbContext.Genders.FirstOrDefault(t => t.Name.Equals("Masculino"));
                var gym = _applicationDbContext.Gyms.FirstOrDefault();
                var birthday = new DateTime(1984, 04, 01);

                await _applicationDbContext.Customers.AddAsync(new Customer
                {
                    User = user,
                    Gender = masculino,
                    Gym = gym,
                    Birthday = birthday
                });
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        private async Task CheckAdminAsync(User user)
        {
            if (!_applicationDbContext.Admins.Any())
            {
                _applicationDbContext.Admins.Add(new Admin { User = user });
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        private async Task CheckGendersAsync()
        {
            if (!_applicationDbContext.Genders.Any())
            {
                await _applicationDbContext.Genders.AddAsync(new Gender { Name = "Femenino" });
                await _applicationDbContext.Genders.AddAsync(new Gender { Name = "Masculino" });
                await _applicationDbContext.Genders.AddAsync(new Gender { Name = "Otro" });
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        private async Task CheckTownsAsync()
        {
            if (!_applicationDbContext.Towns.Any())
            {
                //Select the districts
                var guapiles = _applicationDbContext.Districts.FirstOrDefault(d => d.Name.Equals("Guápiles"));
                var guacimo = _applicationDbContext.Districts.FirstOrDefault(d => d.Name.Equals("Guápiles"));

                //Add the towns
                await _applicationDbContext.Towns.AddAsync(new Town { Name = "Guápiles", District = guapiles });
                await _applicationDbContext.Towns.AddAsync(new Town { Name = "Guácimo", District = guacimo });
            }
        }

        private async Task CheckDistrictsAsync()
        {
            if (!_applicationDbContext.Districts.Any())
            {
                await AddSanJoseProvinceDistritsAsync();
                await AddLimonProvinceDistrictsAsync();
            }
        }

        private async Task CheckCountiesAsync()
        {
            var sanJose = _applicationDbContext.States.FirstOrDefault(p => p.Name.Equals("San José"));
            var alajuela = _applicationDbContext.States.FirstOrDefault(p => p.Name.Equals("Alajuela"));
            var cartago = _applicationDbContext.States.FirstOrDefault(p => p.Name.Equals("Cartago"));
            var heredia = _applicationDbContext.States.FirstOrDefault(p => p.Name.Equals("Heredia"));
            var guanacaste = _applicationDbContext.States.FirstOrDefault(p => p.Name.Equals("Guanacaste"));
            var puntarenas = _applicationDbContext.States.FirstOrDefault(p => p.Name.Equals("Puntarenas"));
            var limon = _applicationDbContext.States.FirstOrDefault(p => p.Name.Equals("Limón"));

            if (!_applicationDbContext.Counties.Any())
            {
                await AddSanJoseCountiesAsync(sanJose);
                await AddAlajuelaCountiesAsync(alajuela);
                await AddCartagoCountiesAsync(cartago);
                await AddHerediaCountiesAsync(heredia);
                await AddGuanacasteCountiesAsync(guanacaste);
                await AddPuntarenasCountiesAsync(puntarenas);
                await AddLimonCountiesAsync(limon);
            }
        }

        private async Task CheckStatesAsync()
        {
            Country country = _applicationDbContext.Countries.FirstOrDefault(p => p.Name.Equals("Costa Rica"));
            if (!_applicationDbContext.States.Any())
            {
                await _applicationDbContext.States.AddAsync(new State { Name = "San José", Country = country });
                await _applicationDbContext.States.AddAsync(new State { Name = "Alajuela", Country = country });
                await _applicationDbContext.States.AddAsync(new State { Name = "Cartago", Country = country });
                await _applicationDbContext.States.AddAsync(new State { Name = "Heredia", Country = country });
                await _applicationDbContext.States.AddAsync(new State { Name = "Guanacaste", Country = country });
                await _applicationDbContext.States.AddAsync(new State { Name = "Puntarenas", Country = country });
                await _applicationDbContext.States.AddAsync(new State { Name = "Limón", Country = country });
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        private async Task CheckCountriesAsync()
        {
            if (!_applicationDbContext.Countries.Any())
            {
                await _applicationDbContext.Countries.AddAsync(new Country { Name = "Costa Rica" });
                await _applicationDbContext.Countries.AddAsync(new Country { Name = "Panamá" });
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        #region AddCounties
        private async Task AddSanJoseCountiesAsync(State state)
        {
            await _applicationDbContext.Counties.AddAsync(new County { Name = "San José", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Escazú", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Desamparados", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Puriscal", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Tarrazú", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Aserrí", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Mora", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Goicoechea", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Santa Ana", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Alajuelita", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Vázquez de Coronado", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Acosta", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Tibás", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Moravia", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Montes de Oca", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Turrubares", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Dota", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Curridabat", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Pérez Zeledón", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "León Cortés Castro", State = state });
            await _applicationDbContext.SaveChangesAsync();
        }

        private async Task AddAlajuelaCountiesAsync(State state)
        {
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Alajuela", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "San Ramón", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Grecia", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "San Mateo", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Atenas", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Naranjo", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Palmares", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Poás", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Orotina", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "San Carlos", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Zarcero", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Sarchí", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Upala", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Los Chiles", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Guatuso", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Río Cuarto", State = state });
            await _applicationDbContext.SaveChangesAsync();
        }

        private async Task AddCartagoCountiesAsync(State state)
        {
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Cartago", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Paraíso", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "La Unión", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Jiménez", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Turrialba", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Alvarado", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Oreamuno", State = state }); await _applicationDbContext.Counties.AddAsync(new County { Name = "San Carlos", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "El Guarco", State = state });
            await _applicationDbContext.SaveChangesAsync();
        }

        private async Task AddHerediaCountiesAsync(State state)
        {
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Heredia", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Barva", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Santo Domingo", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Santa Bárbara", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "San Rafael", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "San Isidro", State = state }); await _applicationDbContext.Counties.AddAsync(new County { Name = "San Carlos", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Belén", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Flores", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "San Pablo", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Sarapiquí", State = state });
            await _applicationDbContext.SaveChangesAsync();
        }

        private async Task AddGuanacasteCountiesAsync(State state)
        {
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Liberia", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Nicoya", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Santa Cruz", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Bagaces", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Carrillo", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Cañas", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Abangares", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Tilarán", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Nandayure", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "La Cruz", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Hojancha", State = state });
            await _applicationDbContext.SaveChangesAsync();
        }

        private async Task AddPuntarenasCountiesAsync(State state)
        {
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Puntarenas", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Esparza", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Buenos Aires", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Montes de Oro", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Osa", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Quepos", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Golfito", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Coto Brus", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Parrita", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Corredores", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Garabito", State = state });
            await _applicationDbContext.SaveChangesAsync();
        }

        private async Task AddLimonCountiesAsync(State state)
        {
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Limón", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Pococí", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Siquirres", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Talamanca", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Matina", State = state });
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Guácimo", State = state });
            await _applicationDbContext.SaveChangesAsync();
        }
        #endregion

        private async Task AddSanJoseProvinceDistritsAsync()
        {
            await AddSanJoseDistrictsAsync();
            /*await AddEscazuDistrictsAsync();
            await AddDesamparadosDistrictsAsync();
            await AddPuriscalDistrictsAsync();
            await AddTarrazuDistrictsAsync();
            await AddAserriDistrictsAsync();
            await AddMoraDistrictsAsync();
            await AddGoicoecheaDistrictsAsync();
            await AddSantaAnaDistrictsAsync();
            await AddAlajuelitaDistrictsAsync();
            await AddVasquezDeCoronadoDistrictsAsync();
            await AddAcostaDistrictsAsync();
            await AddTibasDistrictsAsync();
            await AddMoraviaDistrictsAsync();
            await AddMontesDeOcaDistrictsAsync();
            await AddTurrubaresDistrictsAsync();
            await AddDotaDistrictsAsync();
            await AddCurridabatDistrictsAsync();
            await AddPerezZeledonDistrictsAsync();
            await AddLeonCortesCastroDistrictsAsync();    */
        }

        private async Task AddLimonProvinceDistrictsAsync()
        {
            await AddLimonDistrictsAsync();
            await AddPocociDistrictsAsync();
            await AddSiquirresDistrictsAsync();
            await AddTalamancaDistrictsAsync();
            await AddMatinaDistrictsAsync();
            await AddGuacimoDistrictsAsync();
        }

        #region Limon's (Province) Districts (Ready)
        private async Task AddGuacimoDistrictsAsync()
        {
            var county = _applicationDbContext.Counties.FirstOrDefault(p => p.Name.Equals("Guácimo"));
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Guácimo", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Mercedes", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Pocora", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Río Jiménez", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Duacarí", County = county });
            await _applicationDbContext.SaveChangesAsync();
        }

        private async Task AddMatinaDistrictsAsync()
        {
            var county = _applicationDbContext.Counties.FirstOrDefault(p => p.Name.Equals("Matina"));
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Matina", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Bataán", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Carrandí", County = county });
            await _applicationDbContext.SaveChangesAsync();
        }

        private async Task AddTalamancaDistrictsAsync()
        {
            var county = _applicationDbContext.Counties.FirstOrDefault(p => p.Name.Equals("Talamanca"));
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Bratsi", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Sixaola", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Cahuita", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Telire", County = county });
            await _applicationDbContext.SaveChangesAsync();
        }

        private async Task AddSiquirresDistrictsAsync()
        {
            var county = _applicationDbContext.Counties.FirstOrDefault(p => p.Name.Equals("Siquirres"));
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Siquirres", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Pacuarito", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Florida", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Germania", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "El Cairo", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Alegría", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Reventazón", County = county });
            await _applicationDbContext.SaveChangesAsync();
        }

        private async Task AddPocociDistrictsAsync()
        {
            var county = _applicationDbContext.Counties.FirstOrDefault(p => p.Name.Equals("Pococí"));
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Guápiles", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Jiménez", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "La Rita", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Roxana", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Cariari", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Colorado", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "La Colonia", County = county });
            await _applicationDbContext.SaveChangesAsync();
        }

        private async Task AddLimonDistrictsAsync()
        {
            var county = _applicationDbContext.Counties.FirstOrDefault(p => p.Name.Equals("Limón"));
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Limón", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Valle La Estrella", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Río Blanco", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Matama", County = county });
            await _applicationDbContext.SaveChangesAsync();
        }
        #endregion

        #region San Jose's (Province) Districts
        private Task AddTurrubaresDistrictsAsync()
        {
            throw new NotImplementedException();
        }

        private Task AddDotaDistrictsAsync()
        {
            throw new NotImplementedException();
        }

        private Task AddCurridabatDistrictsAsync()
        {
            throw new NotImplementedException();
        }

        private Task AddPerezZeledonDistrictsAsync()
        {
            throw new NotImplementedException();
        }

        private Task AddLeonCortesCastroDistrictsAsync()
        {
            throw new NotImplementedException();
        }

        private Task AddMontesDeOcaDistrictsAsync()
        {
            throw new NotImplementedException();
        }

        private Task AddTibasDistrictsAsync()
        {
            throw new NotImplementedException();
        }

        private Task AddMoraviaDistrictsAsync()
        {
            throw new NotImplementedException();
        }

        private Task AddVasquezDeCoronadoDistrictsAsync()
        {
            throw new NotImplementedException();
        }

        private Task AddAcostaDistrictsAsync()
        {
            throw new NotImplementedException();
        }

        private Task AddAlajuelitaDistrictsAsync()
        {
            throw new NotImplementedException();
        }

        private Task AddSantaAnaDistrictsAsync()
        {
            throw new NotImplementedException();
        }

        private Task AddGoicoecheaDistrictsAsync()
        {
            throw new NotImplementedException();
        }

        private Task AddMoraDistrictsAsync()
        {
            throw new NotImplementedException();
        }

        private Task AddAserriDistrictsAsync()
        {
            throw new NotImplementedException();
        }

        private Task AddTarrazuDistrictsAsync()
        {
            throw new NotImplementedException();
        }

        private Task AddPuriscalDistrictsAsync()
        {
            throw new NotImplementedException();
        }

        private Task AddDesamparadosDistrictsAsync()
        {
            throw new NotImplementedException();
        }

        private Task AddEscazuDistrictsAsync()
        {
            throw new NotImplementedException();
        }

        private async Task AddSanJoseDistrictsAsync()
        {
            var county = _applicationDbContext.Counties.FirstOrDefault(p => p.Name.Equals("San José"));
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Carmen", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Merced", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Hospital", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Catedral", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Zapote", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "San Francisco de Dos Ríos", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "La Uruca", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Mata Redonda", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Pavas", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "Hatillo", County = county });
            await _applicationDbContext.Districts.AddAsync(new District { Name = "San Sebastián", County = county });
            await _applicationDbContext.SaveChangesAsync();
        }
        #endregion
    }
}