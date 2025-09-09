import React, { useEffect, useState } from 'react';
import { Stage, Layer, Text, Rect } from 'react-konva';

function downloadURI(uri, name) {
  var link = document.createElement('a');
  link.download = name;
  link.href = uri;
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
}

const Image2 = (props) => {

  //const [mandelbrotData, setMandelbrotData] = useState(null);
  const mandelbrotData = props.mandelbrotData;



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
  console.log("circle");
   
  
function clamp(num, min, max) {
  return num <= min 
    ? min 
    : num >= max 
      ? max 
      : num
}

console.log(mandelbrotData);

const rowsJson = mandelbrotData.rowsJson;

// xMin
// xMax
// yMin 
// yMax 
// width 
// height 
const width = mandelbrotData.width;
const height = mandelbrotData.height;
const maxIterations = 250;//mandelbrotData.maxIterations;

console.log(rowsJson.length);
var qweqwe = JSON.parse(rowsJson);
console.log(qweqwe[0]);
console.log(qweqwe.length);
var zxczxc = JSON.parse(qweqwe[0]);
console.log(zxczxc[0]);
console.log(zxczxc.length);
//console.log(JSON.parse(rowsJson[0]));

//provider
//requestedAt
//fininishedAt
   for (let y = 0; y < width; y++) {
    
    var cols = JSON.parse(qweqwe[y]);

    for(let x = 0; x < height; x++) {
      var val = cols[x];

      // if(val < 25)
      //   continue;

      let color;

      if(false) 
      {
        //HSL idea from wiki
        //    https://en.wikipedia.org/wiki/Plotting_algorithms_for_the_Mandelbrot_set#HSV_coloring
        var hsl = [Math.pow((val / maxIterations) * 360, 1.5) % 360, 100, (val / maxIterations) * 100]
        color = `hsl(${hsl[0]}, ${hsl[1]}%, ${hsl[2]}%)`;
      }
      else if(false){
        var hsl = [Math.pow((val / maxIterations) * 360, 1.5) % 360, 60, (val / maxIterations) * 100]
         color = `hsl(${hsl[0]}, ${hsl[1]}%, ${hsl[2]}%)`;
      }
      else {
        if(val < 100)
          continue;

        let colorValue = Math.floor(255 * val / 1000.0);
        colorValue = clamp(colorValue, 0, 255);
        color = `rgb(${colorValue}, ${colorValue}, ${colorValue})`;
      }
    items.push({
       id: `${y}_${x}_${val}`, //val,
       x: y,
       y: x,
       color
     });

    }
   }
   return items;
 }, []);


 const handleMouseMove = (e) => {
  handleMouseOut();

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
       {/* width={window.innerWidth - 20} height={window.innerHeight - 50} */}
       <Stage width={800} height={600}  ref={stageRef}>
           <Layer onClick={handleMouseMove}>
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
                {mandelbrotData != null && circles.map(({ id, x, y, color }) => (
               <Rect listening={false}
                   key={id}
                   x={x}
                   y={y}
                   width={2}
                   height={2}
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


export default Image2;
