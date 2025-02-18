using InputKit.Shared.Validations;
using LocalizationResourceManager.Maui;

namespace TaSked.App.Common.Validations;

public class PickerValidation: IValidation
{
	private readonly ILocalizationResourceManager _localizationResourceManager;
	
	public PickerValidation()
	{
		_localizationResourceManager = ServiceHelper.GetService<ILocalizationResourceManager>();
	}
	
	public string Message { get; set; }
	
	public bool IsRequired { get; set; }
	
	public bool Validate(object value)
	{
		if (IsRequired && value == null)
		{
			Message = _localizationResourceManager["Validation_Required"];
			return false;
		}
		return true;
	}
}