using InputKit.Shared.Validations;
using LocalizationResourceManager.Maui;
using System.ComponentModel;

namespace TaSked.App.Common.Validations;

public class MyTextValidation : IValidation
{
	private readonly ILocalizationResourceManager _localizationResourceManager;

	public MyTextValidation(ILocalizationResourceManager localizationResourceManager)
	{
		_localizationResourceManager = localizationResourceManager;
	}
	
	public string Message { get; set; }
    
	public int MinLength { get; set; }
    
	public int MaxLength { get; set; }

	public bool IsRequired { get; set; }

	public bool Validate(object value)
	{
		if (!IsRequired && string.IsNullOrWhiteSpace(value as string))
		{
			return true;
		}
		if (value is string text)
		{
			if (text.Length < MinLength)
			{
				Message = string.Format(_localizationResourceManager["Validation_MinLength"], MinLength);
				return false;
			}

			if (text.Length > MaxLength)
			{
				Message = string.Format(_localizationResourceManager["Validation_MaxLength"], MaxLength);
				return false;
			}
			return true;
		}
		Message = _localizationResourceManager["Validation_Required"];
		return false;
	}
}