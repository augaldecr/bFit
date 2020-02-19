using bFit.WEB.Data.Entities.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace bFit.Web.Data
{
    public class DbSeeder
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public DbSeeder(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
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
            await _applicationDbContext.Counties.AddAsync(new County { Name = "Oreamuno", State = state });            await _applicationDbContext.Counties.AddAsync(new County { Name = "San Carlos", State = state });
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