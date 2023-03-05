﻿namespace BlazorBootstrap;

public partial class Tooltip : BaseComponent
{
    #region Members

    private string title = default!;

    private TooltipPlacement _placement = TooltipPlacement.Top;

    private DotNetObjectReference<Tooltip> objRef;

    private string placement => Placement.ToTooltipPlacementName();

    #endregion Members

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        this.title = this.Title;
        objRef ??= DotNetObjectReference.Create(this);

        await base.OnInitializedAsync();

        ExecuteAfterRender(async () => { await JS.InvokeVoidAsync("window.blazorBootstrap.tooltip.initialize", ElementId); });
    }

    protected override async Task OnParametersSetAsync()
    {
        if (title != Title)
        {
            await JS.InvokeVoidAsync("window.blazorBootstrap.tooltip.dispose", ElementId);
            await JS.InvokeVoidAsync("window.blazorBootstrap.tooltip.update", ElementId);
        }
    }

    /// <inheritdoc />
    protected override async ValueTask DisposeAsync(bool disposing)
    {
        if (disposing)
        {
            await JS.InvokeVoidAsync("window.blazorBootstrap.tooltip.dispose", ElementId);
            objRef?.Dispose();
        }

        await base.DisposeAsync(disposing);
    }

    #endregion Methods

    #region Properties

    /// <inheritdoc/>
    protected override bool ShouldAutoGenerateId => true;

    /// <summary>
    /// Specifies the content to be rendered inside this.
    /// </summary>
    [Parameter] public RenderFragment ChildContent { get; set; }

    /// <summary>
    /// Displays informative text when users hover, focus, or tap an element.
    /// </summary>
    [Parameter, EditorRequired] public string Title { get; set; }

    /// <summary>
    /// Specifies the tooltip placement. Default is top right.
    /// </summary>
    [Parameter]
    public TooltipPlacement Placement
    {
        get => _placement; 
        set
        {
            _placement = value;
            DirtyClasses();
        }
    }

    #endregion Properties
}
