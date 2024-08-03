class Raylib {
    
    init(dotnetObject, id) {
        const canvas = document.getElementById(id)
        if (canvas) {
            Blazor.runtime.Module['canvas'] = canvas;
            
            if (dotnetObject) {
                Blazor.runtime.Module['canvasInstance'] = dotnetObject;
                window.addEventListener("resize", this.resize, true);
                this.resize({});
            }
        }
    }
    
    async resize(e) {
        const canvas = Blazor.runtime.Module['canvas']
        const dotnetObject =  Blazor.runtime.Module['canvasInstance'];
        
        const dpr = window.devicePixelRatio;
        const width =  canvas.widthNative = canvas.width =  canvas.clientWidth;
        const height = canvas.heightNative = canvas.height =  canvas.clientHeight;


        const { getAssemblyExports } = await globalThis.getDotnetRuntime(0);
        var exports = await getAssemblyExports("Blazor.Raylib.dll");
        exports.Blazor.Raylib.Components.Raylib.ResizeCanvas(dotnetObject, width, height, dpr);
    }
    
    render(dotnetObject, id, fps) {
        if (dotnetObject) {
            const frameCap = 1000.0 / (fps + 16.0);
            let lastTime = performance.now();
            const localRender = async (time) => {
                const { getAssemblyExports } = await globalThis.getDotnetRuntime(0);
                var exports = await getAssemblyExports("Blazor.Raylib.dll");
                
                const now = performance.now();
                let delta = now - lastTime;
                
               if (delta > frameCap) {
                   exports.Blazor.Raylib.Components.Raylib.EventAnimationFrame(dotnetObject, delta);
                   lastTime = now;
               }
                
               requestAnimationFrame(localRender);
            };
            localRender(0);
        }
        else {
            console.log("DotNetReference: ", dotnetObject);
        }
    }
}

export const raylib = new Raylib();