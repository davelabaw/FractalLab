import React from 'react';
import { Stage, Layer, Circle, Text, Rect } from 'react-konva';
//import convert from 'color-convert'; //doing directly


function downloadURI(uri, name) {
 var link = document.createElement('a');
 link.download = name;
 link.href = uri;
 document.body.appendChild(link);
 link.click();
 document.body.removeChild(link);
}

const Image1 = () => {
 const [tooltipProps, setTooltipProps] = React.useState({
   text: '',
   visible: false,
   x: 0,
   y: 0
 });


 const stageRef = React.useRef(null);
  const handleExport = () => {
   const uri = stageRef.current.toDataURL();
   console.log(uri);
   // we also can save uri as file
   downloadURI(uri, 'stage.png');
 };


 const circles = React.useMemo(() => {
   const items = [];
  
   let max = 10000;
   for (let i = 0; i < max; i++) {

     //HSL idea from wiki
     //    https://en.wikipedia.org/wiki/Plotting_algorithms_for_the_Mandelbrot_set#HSV_coloring
     var hsl = [Math.pow((i / max) * 360, 1.5) % 360, 50, (i / max) * 100]
     const color = `hsl(${hsl[0]}, ${hsl[1]}%, ${hsl[2]}%)`;
    
     items.push({
       id: i,
       x: Math.random() * window.innerWidth,
       y: Math.random() * window.innerHeight,
       color
     });
   }
   return items;
 }, []);


 const handleMouseMove = (e) => {
   const mousePos = e.target.getStage().getPointerPosition();
   setTooltipProps({
     text: `node: ${e.target.name()}, color: ${e.target.attrs.fill}`,
     visible: true,
     x: mousePos.x + 5,
     y: mousePos.y + 5
   });
 };

 const handleMouseOut = () => {
   setTooltipProps(prev => ({ ...prev, visible: false }));
 };

 return (
   <>
       <button onClick={handleExport}>Export image</button>
       <Stage width={window.innerWidth - 20} height={window.innerHeight - 50} ref={stageRef}>
           <Layer onMouseMove={handleMouseMove} onMouseOut={handleMouseOut}>
               {/* {circles.map(({ id, x, y, color }) => (
               <Circle
                   key={id}
                   x={x}
                   y={y}
                   radius={1}
                   fill={color}
                   name={id.toString()}
               />
               ))} */}
                {circles.map(({ id, x, y, color }) => (
               <Rect
                   key={id}
                   x={x}
                   y={y}
                   width={1}
                   height={1}
                   fill={color}
                   name={id.toString()}
               />
               ))}
           </Layer>
           <Layer>
               <Text
               {...tooltipProps}
               fontFamily="Calibri"
               fontSize={12}
               padding={5}
               fill="black"
               opacity={0.75}
               />
           </Layer>
       </Stage>
   </>
 );
};


export default Image1;
