using InputKit.Shared.Validations;
using LocalizationResourceManager.Maui;
using System.ComponentModel;

namespace TaSked.App.Common.Validations;

public class UrlValidation : IValidation
{
	private readonly ILocalizationResourceManager _localizationResourceManager;
	
	public UrlValidation()
	{
		_localizationResourceManager = ServiceHelper.GetService<ILocalizationResourceManager>();
	}
	
	public string Message { get; set; }
	
	public bool IsRequired { get; set; }
	
	public bool Validate(object value)
	{
		if (!IsRequired && string.IsNullOrWhiteSpace(value as string))
		{
			return true;
		}
		if (value is string text)
		{
			if (text.Length < 6)
			{
				Message = string.Format(_localizationResourceManager["Validation_MinLength"], 6);
				return false;
			}

			if (text.Length > 2048)
			{
				Message = string.Format(_localizationResourceManager["Validation_MaxLength"], 2048);
				return false;
			}

			if (Uri.TryCreate(text, UriKind.Absolute, out _))
			{
				return true;
			}
			else
			{
				Message = _localizationResourceManager["Validation_InvalidUrl"];
				return false;
			}
		}
		Message = _localizationResourceManager["Validation_Required"];
		return false;
	}
}