using System.Numerics;
using Blazor.Raylib.Extensions;
using Blazor.Raylib.Simple.Services;
using Microsoft.AspNetCore.Components;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;

namespace Blazor.Raylib.Simple.Pages.Playground;

public partial class OpenGL : ComponentBase
{    
    [Inject]
    public required ResourceService ResourceService { get; set; }

    
    private Camera3D _camera;
    
    private async void Init()
    {
        InitWindow(1280,768, "Resize sample");       
        
        RaylibExtensions.SetLoadFileTextCallback(ResourceService.GetLoadedResource);
        RaylibExtensions.SetLoadFileDataCallback(ResourceService.GetLoadedResource);

        await ResourceService.PreloadResource("resources/models/gltf/robot.glb");
        
        _camera.Position = new Vector3(10.0f,10.0f,10.0f);
        _camera.Target = new Vector3(0f, 0f, 0f);
        _camera.Up = new Vector3(0f, 1f, 0f);
        _camera.FovY = 45f;
        _camera.Projection = CameraProjection.Perspective;
    }

    private void Render(float deltaTime)
    {
        UpdateCamera(ref _camera, CameraMode.Orbital);
        Rlgl.EnableBackfaceCulling();
        BeginDrawing();
            ClearBackground(ColorFromHSV(0,0,0));
            DrawFPS(10,10);
                
            BeginMode3D(_camera);
                DrawGrid(100,1);
                //DrawCube(Vector3.Zero,2f,2f,2f, Color.Red);
                
                Rlgl.Begin(0x0009);
                    Rlgl.Vertex3f(0.0f,2.68843232748677f,0.0f);
                    Rlgl.Vertex3f(-0.406565545240771f,2.67741230885222f,0.0f);
                    Rlgl.Vertex3f(-0.811844472420702f,2.64337146165631f,0.0f);
                    Rlgl.Vertex3f(-1.21414523219646f,2.58379966740374f,0.0f);
                    Rlgl.Vertex3f(-1.61084488243579f,2.49439282575701f,0.0f);
                    Rlgl.Vertex3f(-1.99732402407963f,2.36816640911423f,0.0f);
                    Rlgl.Vertex3f(-2.36448287386336f,2.19398404238099f,0.0f);
                    Rlgl.Vertex3f(-2.69269780991348f,1.95519340118616f,0.0f);
                    Rlgl.Vertex3f(-2.94036921021401f,1.63504056083023f,0.0f);
                    Rlgl.Vertex3f(-3.04879990220018f,1.24598157305706f,0.0f);
                    Rlgl.Vertex3f(-3.00744331060112f,0.842991194842604f,0.0f);
                    Rlgl.Vertex3f(-2.86871978591032f,0.461280924370721f,0.0f);
                    Rlgl.Vertex3f(-2.6700397578777f,0.106705825582408f,0.0f);
                    Rlgl.Vertex3f(-2.4291417242145f,-0.220789599923818f,0.0f);
                    Rlgl.Vertex3f(-2.1549737582355f,-0.521038389186614f,0.0f);
                    Rlgl.Vertex3f(-1.85233564258487f,-0.792577607647334f,0.0f);
                    Rlgl.Vertex3f(-1.52394753430662f,-1.03231127678777f,0.0f);
                    Rlgl.Vertex3f(-1.17162752124351f,-1.23516605031891f,0.0f);
                    Rlgl.Vertex3f(-0.797327064970238f,-1.39366465926728f,0.0f);
                    Rlgl.Vertex3f(-0.404494524879168f,-1.49766330003707f,0.0f);
                    Rlgl.Vertex3f(0.0f,-1.5350296491882f,0.0f);
                    Rlgl.Vertex3f(0.404494508793845f,-1.49766330296203f,0.0f);
                    Rlgl.Vertex3f(0.797327043771191f,-1.39366466661145f,0.0f);
                    Rlgl.Vertex3f(1.17162756918928f,-1.23516602630764f,0.0f);
                    Rlgl.Vertex3f(1.52394751766558f,-1.03231128764915f,0.0f);
                    Rlgl.Vertex3f(1.8523356996992f,-0.792577561301781f,0.0f);
                    Rlgl.Vertex3f(2.15497373929206f,-0.52103840795184f,0.0f);
                    Rlgl.Vertex3f(2.42914167878755f,-0.22078965508489f,0.0f);
                    Rlgl.Vertex3f(2.67003976250216f,0.106705832692101f,0.0f);
                    Rlgl.Vertex3f(2.86871977954697f,0.461280910808867f,0.0f);
                    Rlgl.Vertex3f(3.00744329678325f,0.842991139637225f,0.0f);
                    Rlgl.Vertex3f(3.04879990112863f,1.24598158883845f,0.0f);
                    Rlgl.Vertex3f(2.94036920669161f,1.63504056764251f,0.0f);
                    Rlgl.Vertex3f(2.69269779973882f,1.95519341068674f,0.0f);
                    Rlgl.Vertex3f(2.3644828639794f,2.19398404808304f,0.0f);
                    Rlgl.Vertex3f(1.9973240148445f,2.36816641271916f,0.0f);
                    Rlgl.Vertex3f(1.61084487568103f,2.49439282758701f,0.0f);
                    Rlgl.Vertex3f(1.21414531991753f,2.58379965126806f,0.0f);
                    Rlgl.Vertex3f(0.811844476691385f,2.64337146116746f,0.0f);
                    Rlgl.Vertex3f(0.406565563813857f,2.67741230783552f,0.0f);
                Rlgl.End();
                
                Rlgl.Begin(0x0009);
                    Rlgl.Vertex3f(0.0f,2.53099263998677f,0.0f);
                    Rlgl.Vertex3f(0.384274900386849f,2.52103548079205f,0.0f);
                    Rlgl.Vertex3f(0.767439433401587f,2.49028666176807f,0.0f);
                    Rlgl.Vertex3f(1.14803789550381f,2.43649797844599f,0.0f);
                    Rlgl.Vertex3f(1.52382048016624f,2.35580438211911f,0.0f);
                    Rlgl.Vertex3f(1.89083091558087f,2.2418969850467f,0.0f);
                    Rlgl.Vertex3f(2.24125137689733f,2.08456936531179f,0.0f);
                    Rlgl.Vertex3f(2.55793363304913f,1.86795125569967f,0.0f);
                    Rlgl.Vertex3f(2.80260264776878f,1.57383008003324f,0.0f);
                    Rlgl.Vertex3f(2.91499069651162f,1.20929075349148f,0.0f);
                    Rlgl.Vertex3f(2.87781794186053f,0.828439932251812f,0.0f);
                    Rlgl.Vertex3f(2.74333349777552f,0.468976139888769f,0.0f);
                    Rlgl.Vertex3f(2.55041935323226f,0.136804471549583f,0.0f);
                    Rlgl.Vertex3f(2.31718321168942f,-0.168550204261573f,0.0f);
                    Rlgl.Vertex3f(2.0526490842152f,-0.4472871539005f,0.0f);
                    Rlgl.Vertex3f(1.7616701831263f,-0.698307253653909f,0.0f);
                    Rlgl.Vertex3f(1.44705875892003f,-0.918966701176456f,0.0f);
                    Rlgl.Vertex3f(1.11075533913924f,-1.10483174825384f,0.0f);
                    Rlgl.Vertex3f(0.754792575648176f,-1.2493688849428f,0.0f);
                    Rlgl.Vertex3f(0.382480241625981f,-1.34377886037238f,0.0f);
                    Rlgl.Vertex3f(0.0f,-1.37758996454453f,0.0f);
                    Rlgl.Vertex3f(-0.382480094096726f,-1.34377888604729f,0.0f);
                    Rlgl.Vertex3f(-0.754792556281902f,-1.24936889137129f,0.0f);
                    Rlgl.Vertex3f(-1.11075539062433f,-1.10483172351871f,0.0f);
                    Rlgl.Vertex3f(-1.44705874376312f,-0.918966710674264f,0.0f);
                    Rlgl.Vertex3f(-1.76167011118929f,-0.698307309751332f,0.0f);
                    Rlgl.Vertex3f(-2.05264906834f,-0.447287169020584f,0.0f);
                    Rlgl.Vertex3f(-2.31718318258428f,-0.16855023827089f,0.0f);
                    Rlgl.Vertex3f(-2.55041934669669f,0.136804461875579f,0.0f);
                    Rlgl.Vertex3f(-2.74333348315742f,0.468976109786559f,0.0f);
                    Rlgl.Vertex3f(-2.87781793945095f,0.82843992280873f,0.0f);
                    Rlgl.Vertex3f(-2.91499069558299f,1.20929076449465f,0.0f);
                    Rlgl.Vertex3f(-2.80260264225365f,1.57383008985549f,0.0f);
                    Rlgl.Vertex3f(-2.55793362494351f,1.86795126279187f,0.0f);
                    Rlgl.Vertex3f(-2.2412513653713f,2.0845693715988f,0.0f);
                    Rlgl.Vertex3f(-1.89083091723532f,2.24189698444526f,0.0f);
                    Rlgl.Vertex3f(-1.52382036267776f,2.35580441242109f,0.0f);
                    Rlgl.Vertex3f(-1.14803784949771f,2.43649798652405f,0.0f);
                    Rlgl.Vertex3f(-0.767439536078398f,2.49028665054834f,0.0f);
                    Rlgl.Vertex3f(-0.384274901357192f,2.52103548074371f,0.0f);
                Rlgl.End();
                
                Rlgl.Begin(0x0009);
                    Rlgl.Vertex3f(0.0f,2.47792422124388f,-0.939999999999991f);
                    Rlgl.Vertex3f(0.22543570345102f,2.47043703986819f,-0.939999999999991f);
                    Rlgl.Vertex3f(0.449796137172465f,2.44728140008744f,-0.939999999999991f);
                    Rlgl.Vertex3f(0.671650892668505f,2.40670010701811f,-0.939999999999991f);
                    Rlgl.Vertex3f(0.888758496727837f,2.34574313720423f,-0.939999999999991f);
                    Rlgl.Vertex3f(1.09718194276454f,2.25983554212619f,-0.939999999999991f);
                    Rlgl.Vertex3f(1.28945156504883f,2.14242543022066f,-0.939999999999991f);
                    Rlgl.Vertex3f(1.45131083023098f,1.9861702521984f,-0.939999999999991f);
                    Rlgl.Vertex3f(1.56028264873931f,1.78980157903609f,-0.939999999999991f);
                    Rlgl.Vertex3f(1.59834545208795f,1.56846549510419f,-0.93999999999999f);
                    Rlgl.Vertex3f(1.57205129966729f,1.34492794130245f,-0.939999999999991f);
                    Rlgl.Vertex3f(1.50333031384556f,1.13029267864972f,-0.939999999999991f);
                    Rlgl.Vertex3f(1.40585417581734f,0.92698690766237f,-0.939999999999991f);
                    Rlgl.Vertex3f(1.28649651812238f,0.735661983153641f,-0.939999999999991f);
                    Rlgl.Vertex3f(1.1488081683599f,0.557059230027226f,-0.939999999999991f);
                    Rlgl.Vertex3f(0.994572081299554f,0.392537668954707f,-0.939999999999991f);
                    Rlgl.Vertex3f(0.824557792492348f,0.244401234195301f,-0.939999999999991f);
                    Rlgl.Vertex3f(0.639015117223512f,0.116305834057239f,-0.939999999999991f);
                    Rlgl.Vertex3f(0.43825812852227f,0.0138178464649553f,-0.939999999999991f);
                    Rlgl.Vertex3f(0.223741376569279f,-0.0550526474040419f,-0.939999999999991f);
                    Rlgl.Vertex3f(0.0f,-0.0802430054072738f,-0.939999999999991f);
                    Rlgl.Vertex3f(-0.223741369000302f,-0.0550526490804053f,-0.939999999999991f);
                    Rlgl.Vertex3f(-0.438258109334306f,0.0138178384202005f,-0.939999999999991f);
                    Rlgl.Vertex3f(-0.639015107453348f,0.116305828171828f,-0.939999999999991f);
                    Rlgl.Vertex3f(-0.824557778416754f,0.244401223202192f,-0.939999999999991f);
                    Rlgl.Vertex3f(-0.994572071589388f,0.392537659566927f,-0.939999999999991f);
                    Rlgl.Vertex3f(-1.14880815789293f,0.557059217719804f,-0.939999999999991f);
                    Rlgl.Vertex3f(-1.28649650827451f,0.735661969013759f,-0.939999999999991f);
                    Rlgl.Vertex3f(-1.40585419271271f,0.926986938201311f,-0.939999999999991f);
                    Rlgl.Vertex3f(-1.50333030863305f,1.13029266577227f,-0.939999999999991f);
                    Rlgl.Vertex3f(-1.57205128832971f,1.34492789207233f,-0.939999999999991f);
                    Rlgl.Vertex3f(-1.59834545214161f,1.56846549002985f,-0.939999999999991f);
                    Rlgl.Vertex3f(-1.56028266010736f,1.78980154672375f,-0.939999999999991f);
                    Rlgl.Vertex3f(-1.45131082712872f,1.98617025613766f,-0.939999999999991f);
                    Rlgl.Vertex3f(-1.28945159904117f,2.14242540461469f,-0.939999999999991f);
                    Rlgl.Vertex3f(-1.09718192999937f,2.25983554847043f,-0.939999999999991f);
                    Rlgl.Vertex3f(-0.888758510792069f,2.34574313243064f,-0.939999999999991f);
                    Rlgl.Vertex3f(-0.6716508882249f,2.40670010803143f,-0.939999999999991f);
                    Rlgl.Vertex3f(-0.449796135673833f,2.44728140029868f,-0.939999999999991f);
                    Rlgl.Vertex3f(-0.225435788308204f,2.47043703417137f,-0.939999999999991f);
                Rlgl.End();
                
                Rlgl.Begin(0x0009);
                    Rlgl.Vertex3f(0.0f,2.47792422124388f,-0.839999999999991f);
                    Rlgl.Vertex3f(-0.225435703494529f,2.47043703987136f,-0.839999999999991f);
                    Rlgl.Vertex3f(-0.449796137218369f,2.44728140009905f,-0.839999999999991f);
                    Rlgl.Vertex3f(-0.671650892718133f,2.40670010704167f,-0.839999999999991f);
                    Rlgl.Vertex3f(-0.888758496782756f,2.34574313724111f,-0.839999999999991f);
                    Rlgl.Vertex3f(-1.09718194282579f,2.25983554217461f,-0.839999999999991f);
                    Rlgl.Vertex3f(-1.28945156511477f,2.1424254302742f,-0.839999999999991f);
                    Rlgl.Vertex3f(-1.45131083029256f,1.98617025224586f,-0.839999999999991f);
                    Rlgl.Vertex3f(-1.56028264877734f,1.78980157906939f,-0.839999999999991f);
                    Rlgl.Vertex3f(-1.59834545208922f,1.56846549512965f,-0.839999999999991f);
                    Rlgl.Vertex3f(-1.57205129964107f,1.34492794132934f,-0.839999999999991f);
                    Rlgl.Vertex3f(-1.50333031380544f,1.13029267867934f,-0.839999999999991f);
                    Rlgl.Vertex3f(-1.40585417577166f,0.926986907692923f,-0.839999999999991f);
                    Rlgl.Vertex3f(-1.28649651807611f,0.735661983182769f,-0.839999999999991f);
                    Rlgl.Vertex3f(-1.1488081683159f,0.557059230052684f,-0.839999999999991f);
                    Rlgl.Vertex3f(-0.994572081259275f,0.392537668974612f,-0.839999999999991f);
                    Rlgl.Vertex3f(-0.8245577924561f,0.244401234208308f,-0.839999999999991f);
                    Rlgl.Vertex3f(-0.639015117190565f,0.116305834062858f,-0.839999999999991f);
                    Rlgl.Vertex3f(-0.438258128490947f,0.0138178464641787f,-0.839999999999991f);
                    Rlgl.Vertex3f(-0.22374137653742f,-0.0550526474077583f,-0.839999999999991f);
                    Rlgl.Vertex3f(0.0f,-0.0802430054072741f,-0.839999999999991f);
                    Rlgl.Vertex3f(0.22374136903413f,-0.0550526490696816f,-0.839999999999991f);
                    Rlgl.Vertex3f(0.438258109365215f,0.0138178384447853f,-0.839999999999991f);
                    Rlgl.Vertex3f(0.639015107478721f,0.116305828210529f,-0.839999999999991f);
                    Rlgl.Vertex3f(0.824557778435065f,0.244401223253757f,-0.839999999999991f);
                    Rlgl.Vertex3f(0.994572071600111f,0.392537659629489f,-0.839999999999991f);
                    Rlgl.Vertex3f(1.14880815789634f,0.557059217791287f,-0.839999999999991f);
                    Rlgl.Vertex3f(1.2864965082716f,0.735661969092022f,-0.839999999999991f);
                    Rlgl.Vertex3f(1.40585419270523f,0.926986938284213f,-0.839999999999991f);
                    Rlgl.Vertex3f(1.50333030862371f,1.13029266585776f,-0.839999999999991f);
                    Rlgl.Vertex3f(1.57205128832256f,1.34492789215875f,-0.839999999999991f);
                    Rlgl.Vertex3f(1.59834545214197f,1.56846549011699f,-0.839999999999991f);
                    Rlgl.Vertex3f(1.56028266011557f,1.78980154681359f,-0.839999999999991f);
                    Rlgl.Vertex3f(1.45131082713423f,1.98617025622727f,-0.839999999999991f);
                    Rlgl.Vertex3f(1.28945159903547f,2.14242540469453f,-0.839999999999991f);
                    Rlgl.Vertex3f(1.09718192998164f,2.25983554853335f,-0.839999999999991f);
                    Rlgl.Vertex3f(0.888758510764901f,2.34574313247461f,-0.839999999999991f);
                    Rlgl.Vertex3f(0.671650888191183f,2.4067001080579f,-0.839999999999991f);
                    Rlgl.Vertex3f(0.449796135636075f,2.44728140031113f,-0.839999999999991f);
                    Rlgl.Vertex3f(0.225435788267922f,2.47043703417465f,-0.839999999999991f);                
                Rlgl.End();
           EndMode3D();
       EndDrawing();
    }

    private void OnResize((int width, int height) Size)
    {
        SetWindowSize(Size.width, Size.height);
    }
}

//https://ifc43-docs.standards.buildingsmart.org/IFC/RELEASE/IFC4x3/HTML/annex_e/advanced-geometric-shape/basin-faceted-brep.html#584