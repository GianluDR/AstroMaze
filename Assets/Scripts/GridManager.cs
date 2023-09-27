using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Security.Cryptography;

public class GridManager : MonoBehaviour
{
    public Grid myGrid;
    public Tilemap[] myTilemaps;
    public Tile[] myTiles;
    public AnimatedTile exitTile;
    public Vector2 exit;
    public Vector2 start;
    public Vector2 startWorld;
    public int dimX;
    public int dimY;
    public string seed;
    public GameObject pointParent;
    public GameObject point;
    public LayerMask obstacleLayer;
    public List<Vector2> visitedTiles = new List<Vector2>();
    public Queue q;

    public int offsetX;
    public int offsetY;

    private int nToSolve = 0;

    //private int maxDim = 99;

    void Start()
    {
        createMap2();
    }

    private Vector2 v = Vector2.zero;
    private bool flag = true;
    private Queue exploreTilemap(Queue myQ){
        Vector2 pos = (Vector2) myQ.Dequeue();
        if(pos == v)
            flag = true;
        //Right
        for(int i = (int)pos.x; i < dimX+1; i++){
            TileBase t = myTilemaps[0].GetTile(new Vector3Int(i+offsetX,((int)pos.y)+offsetY));
            myTilemaps[1].SetTile(new Vector3Int(i+offsetX,((int)pos.y)+offsetY),myTiles[2]);
            if(new Vector2(i,pos.y) == exit){
                myQ.Enqueue(new Vector2(i,pos.y));
                return myQ;
            }
            if(t != null){
                Vector2 posToQ = new Vector2(i-1,((int)pos.y));
                if(!myQ.Contains(posToQ) && !visited(posToQ)){
                    myQ.Enqueue(posToQ);
                    visitedTiles.Add(posToQ);
                    if(flag && !pathExist){
                        flag = false;
                        v = posToQ;
                        nToSolve++;
                    }
                }
                i = dimX+1;
            }
        }
        
        //Left
        for(int i = (int)pos.x; i > -2; i--){
            TileBase t = myTilemaps[0].GetTile(new Vector3Int(i+offsetX,((int)pos.y)+offsetY));
            myTilemaps[1].SetTile(new Vector3Int(i+offsetX,((int)pos.y)+offsetY),myTiles[2]);
            if(new Vector2(i,pos.y) == exit){
                myQ.Enqueue(new Vector2(i,pos.y));
                return myQ;
            }
            if(t != null){
                Vector2 posToQ = new Vector2(i+1,((int)pos.y));
                if(!myQ.Contains(posToQ) && !visited(posToQ)){
                    myQ.Enqueue(posToQ);
                    visitedTiles.Add(posToQ);
                    if(flag && !pathExist){
                        flag = false;
                        v = posToQ;
                        nToSolve++;
                    }
                }
                i = -2;
            }
        }
        //Up
        for(int j = (int)pos.y; j < dimY+1; j++){
            TileBase t = myTilemaps[0].GetTile(new Vector3Int(((int)pos.x)+offsetX,j+offsetY));
            myTilemaps[1].SetTile(new Vector3Int(((int)pos.x)+offsetX,j+offsetY),myTiles[2]);
            if(new Vector2(pos.x,j) == exit){
                myQ.Enqueue(new Vector2(pos.x,j));
                return myQ;
            }
            if(t != null){
                Vector2 posToQ = new Vector2(((int)pos.x),j-1);
                if(!myQ.Contains(posToQ) && !visited(posToQ)){
                    myQ.Enqueue(posToQ);
                    visitedTiles.Add(posToQ);
                    if(flag && !pathExist){
                        flag = false;
                        v = posToQ;
                        nToSolve++;
                    }
                }
                j = dimY+1;
            }
        }
        //Down
        for(int j = (int)pos.y; j > -2; j--){
            TileBase t = myTilemaps[0].GetTile(new Vector3Int(((int)pos.x)+offsetX,j+offsetY));
            myTilemaps[1].SetTile(new Vector3Int(((int)pos.x)+offsetX,j+offsetY),myTiles[2]);
            if(new Vector2(pos.x,j) == exit){
                myQ.Enqueue(new Vector2(pos.x,j));
                return myQ;
            }
            if(t != null){
                Vector2 posToQ = new Vector2(((int)pos.x),j+1);
                if(!myQ.Contains(posToQ) && !visited(posToQ)){
                    myQ.Enqueue(posToQ);
                    visitedTiles.Add(posToQ);
                    if(flag && !pathExist){
                        flag = false;
                        v = posToQ;
                        nToSolve++;
                    }
                }
                j = -2;
            }
        }
        return myQ;
    }

    private bool pathExist;
    private int mapSolution(){
        int i = 0;
        int n = -1;
        pathExist = false;
        Queue myQ = new Queue();
        //Tile startTile = (Tile) myTilemaps[1].GetTile(new Vector3Int((int)start.x+offsetX,(int)start.y+offsetY));
        //Tile exitTile = (Tile) myTilemaps[2].GetTile(new Vector3Int((int)exit.x+offsetX,(int)exit.y+offsetY));
        myQ.Enqueue(start);
        visitedTiles.Add(start);
        while(myQ.Count > 0 && i < 500){
            myQ = exploreTilemap(myQ);
            if(myQ.Contains(exit) && !pathExist){
                n = i;
                pathExist = true;
            }
            i++;
        }
        //Debug.Log(myQ.Count);
        return n;
    }

    public void createMap2(){
        if(dimX < 20)
            dimX++;
        if(dimY < 20)
            dimY++;
        offsetX = (dimX/2) * -1;
        offsetY = (dimY/2) * -1;
        int n;
        do{
            n = 0;
            nToSolve = 0;
            for(int i = 0;i < dimX;i++){
                for(int j = 0;j < dimY;j++){
                    if(i == 0){
                        myTilemaps[0].SetTile(new Vector3Int(i+(offsetX-1),j+offsetY,0),myTiles[0]);
                    }
                    if(j == 0){
                        myTilemaps[0].SetTile(new Vector3Int(i+offsetX,j+(offsetY-1),0),myTiles[0]);
                    }
                    if(i == dimX-1){
                        myTilemaps[0].SetTile(new Vector3Int(i+(offsetX+1),j+offsetY,0),myTiles[0]);
                        
                    }
                    if(j == dimY-1){
                        myTilemaps[0].SetTile(new Vector3Int(i+offsetX,j+(offsetY+1),0),myTiles[0]);
                    }
                    if(Random.value < 0.12){
                        myTilemaps[0].SetTile(new Vector3Int(i+offsetX,j+offsetY,0),myTiles[0]);
                    }else{
                        myTilemaps[1].SetTile(new Vector3Int(i+offsetX,j+offsetY,0),myTiles[1]);
                    }
                }
            }
            do{
                n = 0;
                int startPosX = RandomNumberGenerator.GetInt32(0,dimX-1);
                int exitPosX = RandomNumberGenerator.GetInt32(0,dimX-1);
                int startPosY = RandomNumberGenerator.GetInt32(0,dimY-1);
                int exitPosY = RandomNumberGenerator.GetInt32(0,dimY-1);
                start = new Vector2(startPosX,startPosY);
                exit = new Vector2(exitPosX,exitPosY);
                TileBase tileStart = myTilemaps[0].GetTile(new Vector3Int(((int)start.x)+offsetX,((int)start.y)+offsetY));
                if(tileStart != null)
                    n = -1;
                TileBase tileExit = myTilemaps[0].GetTile(new Vector3Int(((int)exit.x)+offsetX,((int)exit.y)+offsetY));
                if(tileExit != null)
                    n = -1;
            }while(n == -1);
            startWorld = myGrid.CellToWorld(new Vector3Int(((int)start.x)+offsetX,((int)start.y)+offsetY));
            startWorld.x = startWorld.x + 0.5f;
            startWorld.y = startWorld.y + 0.5f;
            myTilemaps[2].SetTile(new Vector3Int(((int)exit.x)+offsetX,((int)exit.y)+offsetY),exitTile);
            if(n != -1)
                n = mapSolution();
            
            
            if(n != -1){
                setPoints();
                createSeed();
                if((percentWall()-((dimX*dimY)/2)) >= 0){
                    n = -1;
                }
            }
            if(n == -1 || n == 0){
                //Vector2 newExit = getDistantVisited();
                //myTilemaps[2].SetTile(new Vector3Int(((int)exit.x)+offsetX,((int)exit.y)+offsetY),null);
                //exit = newExit;
                //myTilemaps[2].SetTile(new Vector3Int(((int)exit.x)+offsetX,((int)exit.y)+offsetY),myTiles[6]);
                reset();
                n = -1;
            
            }
        }while(n == -1);
        GameObject player = GameObject.FindWithTag("Player");
        //Debug.Log(nToSolve+3);
        player.GetComponent<TrailRenderer>().Clear();
        player.GetComponent<TrailRenderer>().enabled = false;
        player.GetComponent<TrailRenderer>().enabled = true;
        player.transform.position = new Vector3Int(((int)startWorld.x),((int)startWorld.y));
        if(player.GetComponent<PlayerController>() != null)
            player.GetComponent<PlayerController>().stopMoving();
        if(player.GetComponent<PlayerMode2>() != null){
            player.GetComponent<PlayerMode2>().stopMoving();
            player.GetComponent<PlayerMode2>().setNToSolve(nToSolve+4);
        }
    }

    private void setPoints(){
        int points = 0;
        int dimPoint =(int) dimY / 4;
        for(int i = 1;i < visitedTiles.Count && points < dimPoint;i++){
            if(Random.value < 0.2){
                Vector2 cellPos = visitedTiles[i];
                Vector3 pos = myGrid.CellToWorld(new Vector3Int((int)cellPos.x+offsetX,(int)cellPos.y+offsetY));
                pos = new Vector3(pos.x+0.5f,pos.y+0.5f,0);
                Instantiate(point,pos,Quaternion.identity,pointParent.transform);
                points++;
            }
        } 
    }

    private bool visited(Vector2 pos){
        for(int i = 0; i < visitedTiles.Count; i++){
            if(pos == visitedTiles[i]){
                return true;
            }
        }
        return false;
    }

    private Vector2 getDistantVisited(){
        float max = 0;
        Vector2 v = Vector2.zero;
        for(int i = 0; i < visitedTiles.Count; i++){
            float distance = Vector3.Distance(visitedTiles[i],startWorld);
            if(distance > max){
                max = distance;
                v = visitedTiles[i];
            }
        }
        return v;
    }

    private bool pointPosCheck(Vector3 pos){
        for(int i = 0; i < pointParent.transform.childCount; i++){
            if(Vector3.Distance(pos,pointParent.transform.GetChild(i).position) == 0)
                return false;
        }
        return true;
    }

    public void reset(){
        for(int i = -2;i < dimX+2;i++){
            for(int j = -2;j < dimY+2;j++){
                myTilemaps[0].SetTile(new Vector3Int(i+offsetX,j+offsetY,0),null);
                myTilemaps[1].SetTile(new Vector3Int(i+offsetX,j+offsetY,0),null);
                myTilemaps[2].SetTile(new Vector3Int(i+offsetX,j+offsetY,0),null);
            }
        }
        for(int i = 0; i < pointParent.transform.childCount; i++){
            Destroy(pointParent.transform.GetChild(i).gameObject); 
        }
        visitedTiles = new List<Vector2>();
    }

    private void createSeed(){
        for(int i = -1;i < dimX+1;i++){
            for(int j = -1;j < dimY+1;j++){
                TileBase tileWall = myTilemaps[0].GetTile(new Vector3Int(i+offsetX,j+offsetY));
                Tile tileFloor = (Tile) myTilemaps[1].GetTile(new Vector3Int(i+offsetX,j+offsetY));
                //Debug.Log(seed);
                if(tileWall != null){
                    seed = seed + "w";
                }else if(tileFloor == myTiles[6]){
                    seed = seed + "o";
                }else if(tileFloor == myTiles[1]){
                    myTilemaps[0].SetTile(new Vector3Int(i+offsetX,j+offsetY,0),myTiles[0]);
                    seed = seed + "w";
                }else if(tileFloor == myTiles[2]){
                    seed = seed + "f";
                }
            }
        }
    }

    public void createFromSeed(string seed){
        int n = 0;
        for(int i = -1;i < dimX+1;i++){
            for(int j = -1;j < dimY+1;j++){
                char tile = seed[n];
                n++;
                if(tile == 'w'){
                    myTilemaps[0].SetTile(new Vector3Int(i+(offsetX),j+offsetY,0),myTiles[0]);
                }else if(tile == 'f'){
                    myTilemaps[1].SetTile(new Vector3Int(i+(offsetX),j+offsetY,0),myTiles[1]);
                }else{
                    myTilemaps[1].SetTile(new Vector3Int(i+(offsetX),j+offsetY,0),myTiles[3]);
                }
            }
        }
    }

    private int percentWall(){
        int n = 0;
        for(int i = 0;i < dimX;i++){
            for(int j = 0;j < dimY;j++){
                TileBase tileWall = myTilemaps[0].GetTile(new Vector3Int(i+offsetX,j+offsetY));
                //Debug.Log(seed);
                if(tileWall != null){
                    n++;
                }
            }
        }
        return n;
    }
}
