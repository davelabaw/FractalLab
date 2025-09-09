import React, { useEffect, useState } from 'react';
import Image2 from './Image2';

const FractalImage = () => {
  const [data, setData] = useState(null);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(true);

    useEffect(() => {

        const fetchData = async () => {
            try {
                //const minX = -2.5, 
                // maxX = 1.0, 
                // minY = -1.0, 
                // maxY = 1.0, 
                // width = 800, 
                // height = 600, 
                // maxIterations = 150;
                
                const minX = 0.27379190654919733,  
                        maxX = 0.2762161342610228,
                        minY =  0.005459365230407022, 
                        maxY = 0.00684463820859303,
                        width = 800, 
                        height = 600, 
                        maxIterations = 10400;
                const endpoint = `https://localhost:32779/fractal/mandelbrot-set/${minX}/${maxX}/${minY}/${maxY}/${width}/${height}/${maxIterations}`
                const response = await fetch(endpoint); 
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                const result = await response.json();
                setData(result);
            } catch (err) {
                setError(err);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
        }, []); // Empty dependency array means this runs once on mount

    if (loading) return <div>Loading...</div>;
    if (error) return <div>Error: {error.message}</div>;    

    return (
        <>
            <Image2 mandelbrotData={data} />
        </>
     );
};


export default FractalImage;
