namespace BlazorPreRendering.Client.Shared;

public partial class PreRenderingIndicator
{
    private bool IsPreRendering = true;

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);
        if (firstRender)
        {
            IsPreRendering = false;
            StateHasChanged();
        }
    }
}