using bFit.Web.Data.Entities.Profiles;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Data.Entities.PersonalData
{
    public class DataTake : IEntity
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Trainer")]
        public Trainer Trainer { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha de toma")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Altura (cm)")]
        public double Height { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Peso (KG)")]
        public double Weight { get; set; }

        //TODO: Ingresar data de los usuario del gimnasio
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Masa músculo-esquelética (KG)")]
        public double MuscleEsquelethicalMassKG { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Masa de grasa (KG)")]
        public double FatMassKG { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Masa de agua (KG)")]
        public double WaterMass { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Edad metabólica (años)")]
        public int MetabolicAge { get; set; }

        //measurements (Medidas)
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Medida de pecho-espalda (cm)")]
        public double ChestBack { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Medida de cintura (cm)")]
        public double Waist { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Medida de abdomen (cm)")]
        public double Abdomen { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Medida de cadera (cm)")]
        public double Hip { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Medida de brazo izquierdo (cm)")]
        public double LeftArm { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Medida de brazo derecho (cm)")]
        public double RightArm { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Medida de cuadriceps izquierdo (cm)")]
        public double LeftQuadriceps { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Medida de cuadriceps derecho (cm)")]
        public double RightQuadriceps { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Medida de pantorrilla izquierda (cm)")]
        public double LeftCalf { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Medida de pantorrilla derecha (cm)")]
        public double RightCalf { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Medida de antebrazo izquierdo (cm)")]
        public double LeftForearm { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Medida de antebrazo derecho (cm)")]
        public double RightForearm { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Nivel de grasa visceral")]
        public double VisceralFatLevel { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Taza metabólica basal (Kcal)")]
        public int BasalMetabolicRate { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Ingesta diaria recomendada de calorías (Kcal)")]
        public int RecommendedCaloricIntake { get; set; }

        //TODO: Desarrollar fórmula
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Índice de masa corporal")]
        public double BodyMassIndex => 23.3;

        //TODO: Desarrollar fórmula
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Porcentaje de grasa corporal")]
        public double FatPercentage => 23.3;

        //TODO: Desarrollar fórmula
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Nivel de obesidad")]
        public ObesityLevel ObesityLevel { get; set; }
    }
}
