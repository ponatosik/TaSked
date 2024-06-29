using CommunityToolkit.Maui.Animations;

namespace TaSked.App;

public class ScaleAnimation : BaseAnimation
{
    public async Task Animate(VisualElement view, bool isVisible)
    {
        if (isVisible)
        {
            await view.TranslateTo(0, -view.Height, 0, Easing.SpringOut);
            view.IsVisible = true;
            await view.TranslateTo(0, 0, Length, Easing.SpringOut);
        }
        else if (!isVisible)
        {
            await view.TranslateTo(0, -view.Height, Length, Easing.SpringIn);
            view.IsVisible = false;
        }
    }

    public override async Task Animate(VisualElement view, CancellationToken token = default)
    {
        await Animate(view, view.IsVisible);
    }
}