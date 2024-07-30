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
}