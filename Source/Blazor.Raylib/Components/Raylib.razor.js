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
    
    render(dotnetObject, id) {
        if (dotnetObject) {
            let lastTime = performance.now();
            const localRender = async (time) => {
                const { getAssemblyExports } = await globalThis.getDotnetRuntime(0);
                var exports = await getAssemblyExports("Blazor.Raylib.dll");
                let delta = time - lastTime;
                if (!delta)
                    delta = 0;
                exports.Blazor.Raylib.Components.Raylib.EventAnimationFrame(dotnetObject, delta);
                lastTime = time;
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