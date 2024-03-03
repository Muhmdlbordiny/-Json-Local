using System.ComponentModel.DataAnnotations;

namespace json_based_localization.ViewModel
{
    public class CreateTestViewModel
    {
        [Display(Name="name"),Required(ErrorMessage ="required")]
      public string Name { get; set; }
    }
}
