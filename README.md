# Terraformer
this is a technical project on landmass generation i made to improve my skills in game developement

## Landmass generation
each landmass chunk is generated by first making a square of triangles, after that height is calculated for each vertex of the tringles that make the mesh.
the height is calculated using different layers of noise
![alt text](https://github.com/MatteoLecchi1/Terraformer/blob/main/Screenshots/AChunk.PNG?raw=true)

## Chunks
landmass chunks are generated based on the distance from the player,
if the player gets far enoght away from a loaded chunk this is hidden but is still loaded to avoid having to reload said chunk
![alt text](https://github.com/MatteoLecchi1/Terraformer/blob/main/Screenshots/Chunks.PNG?raw=true)

## water
water has animated textures and some physics to allow the player to land in it
![alt text](https://github.com/MatteoLecchi1/Terraformer/blob/main/Screenshots/FinalProduct.PNG?raw=true)
