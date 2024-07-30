class Raylib {
    
    init(id) {
        Blazor.runtime.Module['canvas'] = document.getElementById(id) 
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