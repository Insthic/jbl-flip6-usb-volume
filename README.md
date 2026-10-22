# JBL Flip 6 USB Volume

Control de volumen por software para un JBL Flip 6 conectado por USB y reconocido por Windows como **Zgmicro AUDIO**.

Cuando el parlante suena al máximo aunque Windows indique 0 %, esta herramienta refleja el volumen principal en las sesiones de las aplicaciones que reproducen por ese dispositivo. Incluye un control en la bandeja y una opción para reducir adicionalmente los sonidos de Windows.

## Descargar y usar

1. [Descargar JBLVolume.exe](https://github.com/Insthic/jbl-flip6-usb-volume/raw/refs/heads/main/JBLVolume.exe).
2. Guardarlo en una carpeta con permiso de escritura y ejecutarlo con el JBL conectado como Zgmicro AUDIO.
3. Usar el volumen de Windows, las teclas multimedia o el control de la aplicación.

Doble clic en el icono de la bandeja para abrir la ventana. Cerrar la ventana la oculta; **Salir (conservar volumen actual)** detiene el programa sin restaurar niveles que podrían resultar fuertes.

El menú incluye **Iniciar con Windows** (desactivado por defecto) y **Sonidos de Windows suaves** (activado: volumen de los sonidos del sistema al 5 % del nivel principal).

## Compatibilidad

- Windows 10/11 de 64 bits y .NET Framework 4.8.
- Parlante conectado por USB y reconocido como **Zgmicro AUDIO**. La aplicación busca por nombre y rechaza una selección ambigua si encuentra varios.
- No requiere permisos de administrador, REA, Ghidra, Java ni Node.js para ejecutarse.
- Probado en un equipo con JBL Flip 6. El usuario confirmó que el control de las aplicaciones funciona. El ajuste posterior de sonidos del sistema se verificó mediante lectura de los niveles de sesión; no se midió su salida acústica.
- No se comprobó en otros equipos ni en otros modelos de JBL.

## Cómo funciona y límites

Lee `IAudioEndpointVolume` del Zgmicro y ajusta `ISimpleAudioVolume` de sus sesiones aproximadamente cada 30 ms. Conserva los niveles relativos de las aplicaciones en lo posible y guarda estado junto al ejecutable para evitar atenuación acumulativa al reiniciar.

Funciona sobre el mezclador de audio compartido de Windows. Una aplicación que utilice WASAPI exclusivo o ASIO puede evitar ese mezclador. Una sesión nueva puede emitir sonido antes del siguiente ciclo de ajuste: **esta herramienta no es un limitador de seguridad**. No controla el volumen físico ni modifica el firmware del parlante.

No cambia la salida de audio predeterminada. Si se desconecta el USB, espera a que vuelva a conectarse. Solo administra las sesiones del Zgmicro; el volumen por aplicación puede cambiar mientras está activo.

Al llevar el programa a otra PC, copiar **solo el ejecutable**; no copiar `session-levels.tsv`. Los diagnósticos y el estado se crean localmente y no se envían a ningún servidor.

## Compilar

Desde PowerShell, en la carpeta del proyecto:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\build.ps1
```

El script utiliza el compilador de .NET Framework incluido en Windows. No descarga dependencias. El resultado es `JBLVolume.exe`.

Para generar un diagnóstico de solo lectura:

```powershell
.\JBLVolume.exe --diagnose
Get-Content .\diagnostic.txt
```

## Desinstalar

Desactivar **Iniciar con Windows**, salir desde la bandeja y eliminar la carpeta. No se instala ningún controlador.

## Investigación

Windows identificó el controlador de clase USB Audio de Microsoft utilizado por el dispositivo; se inspeccionó ese archivo con REA 6.7.0 y Ghidra 12.1.4. Esa inspección no estableció la causa exacta en el firmware. Las mediciones con las interfaces de audio mostraron un volumen principal de cero y sesiones independientes al máximo; la solución utiliza la atenuación de estas sesiones.

Referencias: [controles de hardware del endpoint](https://learn.microsoft.com/en-us/windows/win32/api/endpointvolume/nf-endpointvolume-iaudioendpointvolume-queryhardwaresupport), [volumen de sesión](https://learn.microsoft.com/en-us/windows/win32/api/audioclient/nf-audioclient-isimpleaudiovolume-setmastervolume).

Proyecto independiente, sin afiliación con JBL, Harman ni Microsoft. Código bajo licencia MIT. El ejecutable no tiene firma digital.
