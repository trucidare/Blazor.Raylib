using Microsoft.AspNetCore.Components;

namespace Blazor.Raylib.Simple.Components.Icon;

public static class MaterialDesignIcons
{
    public static MarkupString ContainStart = new(
        "<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 24 24\"><path fill=\"currentColor\" d=\"M2 3h6v2H4v14h4v2H2zm5 14v-2h2v2zm4 0v-2h2v2zm4 0v-2h2v2z\"/></svg>"
    );

    public static MarkupString Installation = new(
        "<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 2048 2048\"><path fill=\"currentColor\" d=\"M1664 1664h128v384H128v-384h128v256h1408zm-147-531l-557 557l-557-557l90-90l403 402V128h128v1317l403-402z\"/></svg>"
    );

    public static MarkupString Window = new(
        "<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 24 24\"><path fill=\"currentColor\" d=\"M5.616 20q-.672 0-1.144-.472T4 18.385V5.615q0-.67.472-1.143Q4.944 4 5.616 4h12.769q.67 0 1.143.472q.472.472.472 1.144v12.769q0 .67-.472 1.143q-.472.472-1.143.472zm6.884-7.5V19h5.885q.269 0 .442-.173t.173-.442V12.5zm0-1H19V5.616q0-.27-.173-.443T18.385 5H12.5zm-1 0V5H5.616q-.27 0-.443.173T5 5.616V11.5zm0 1H5v5.885q0 .269.173.442t.443.173H11.5z\"/></svg>"
        );

    public static MarkupString Playground = new(
        "<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 15 15\"><path fill=\"currentColor\" d=\"M3 1.15a1.5 1.5 0 1 1 2.903.757A1.5 1.5 0 0 1 3 1.15m11 11.17a1 1 0 0 1-.796 1.17H13.2a1 1 0 0 1-1.07-.49l-1.68-3.37L9 9.92l-.22.08h-.06v2.15l.62-.15h.14a.52.52 0 0 1 .19 1l-5 1a.51.51 0 0 1-.17 0a.52.52 0 0 1-.2-1l4.15-.83V10l-3.22.58a1 1 0 0 1-1.21-.68H4L3 5.83a1 1 0 0 1 0-.43a1 1 0 0 1 .8-.75l4.7-.52V0h.22v4.1h.06L9 4.08L9.4 4h.21a.5.5 0 0 1 .37.6a.49.49 0 0 1-.49.4L9 5.08h-.28v2.86h.06L9 7.88l1.81-.36a1 1 0 0 1 1 .6l2 3.94a.999.999 0 0 1 .19.26M8.5 5.13L6 5.4l.74 2.94L8.5 8z\"/></svg>"
    );
}