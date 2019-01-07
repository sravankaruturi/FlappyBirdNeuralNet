using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /* Game Stuff */
    public Button startGameButton;
    public GameObject pipePrefab;
    public GameObject playerPrefab;
    public GameObject pipeList;
    public GameObject playerList;

    public Text generationText;

    /* Pipe Stuff */
    [SerializeField]
    float max_top;
    [SerializeField]
    float min_top;
    [SerializeField]
    float space_between_top_and_bot;

    [SerializeField]
    float screenTop = 6.0f;
    [SerializeField]
    float screenBot = -6.0f;

    [SerializeField]
    float x_value;

    /* Neural Net stuff */

    [SerializeField]
    int generationNumber = 0;
    [SerializeField]
    int populationSize = 50;

    int remainingAliveBirds = 0;
    public Text remainingBirdsText;

    public Text maxScoreText;

    float maxScore;
    int maxScoreGeneration;

    private int[] layers = { 5, 5, 2, 1 };

    private List<Player> players = null;

    [SerializeField]
    int number_of_players_per_generation = 50;

    private List<NeuralNetworkTest> nets;

    public bool gameRunning = false;
    public bool gameFinished = false;

    float timer = 0.0f;

    [SerializeField]
    float jumpCoolDown = 0.5f;

    [SerializeField]
    float score = 0.0f;
    int scoreInt = 0;

    System.Random random;

    double[] randomFloats = { 0.7421522114912509, 0.32621089178049134, 0.4813629819108294, 0.7092229470562533, 0.009771695133634406, 0.31740374740780786, 0.4643012898111426, 0.2495973505123117, 0.27200504635735523, 0.307829133204957, 0.6029559643907878, 0.1834835693627771, 0.1193639605525969, 0.22674831456775824, 0.29989909160873685, 0.1243262750973283, 0.8449780605167572, 0.7396109137159596, 0.0303601955914653, 0.7405961848753039, 0.9719130783400908, 0.9449628632270782, 0.6155910146468109, 0.6954175835949329, 0.9270919312562703, 0.421013978532948, 0.06598456161484112, 0.7584210577140871, 0.5009725057562822, 0.24652104687374743, 0.9162946584964419, 0.04119896331688955, 0.42723736998417616, 0.9250426634918382, 0.7304243453328689, 0.024901161496106106, 0.49655946515085536, 0.9706695445014341, 0.9188151636273927, 0.877305207898759, 0.8408303516151593, 0.2659992899398623, 0.5332524394095783, 0.9877417047853837, 0.46641348930815796, 0.5362700071562334, 0.6674671910398791, 0.7562063560666494, 0.6383729581204359, 0.5283601404904132, 0.712360902395789, 0.09764189009875412, 0.29408504256031753, 0.9646692917756925, 0.9907904586902788, 0.05404051264145182, 0.9757974239551122, 0.9496334206207798, 0.5435416492494423, 0.0287156438014311, 0.032051458414193656, 0.29239739649066465, 0.9312453309626244, 0.4161108380431311, 0.6571997983516319, 0.6876350342410542, 0.7095633145828554, 0.6895651962879139, 0.0038448805552009357, 0.7746419485052096, 0.3370798870509819, 0.1923146337669952, 0.49299611342962857, 0.5834933775349459, 0.40484817532120454, 0.15735169559352047, 0.030445523135788966, 0.8698916817096884, 0.8692702645983009, 0.30280625756709445, 0.8517791404940912, 0.8605663649178303, 0.4759990488794674, 0.4802080496670432, 0.09680775486264936, 0.9700234540407815, 0.14321927639210152, 0.5956323517005008, 0.7619790082350925, 0.7100399327989313, 0.5322654171582011, 0.9734402125265689, 0.8781134379476946, 0.9079963019281165, 0.8081475539545365, 0.9709319295712197, 0.0037037347103575646, 0.3382109532400289, 0.62040664396057, 0.9505853070785416, 0.6883395525110295, 0.1412363152140571, 0.9039444502474561, 0.21766933869686522, 0.5987596244764783, 0.7331198843301228, 0.2506911557175904, 0.1275259694945775, 0.11458084666841006, 0.8650096881569831, 0.11495222386695114, 0.05228574989445045, 0.8828741183642554, 0.09339107830682869, 0.3127151727697234, 0.9048821090406881, 0.2687655110470447, 0.3880906874412392, 0.37568596978102964, 0.6056274152613875, 0.6624472276837874, 0.392088674119343, 0.46308330509277806, 0.26235329019983933, 0.7406857067879172, 0.941168519534489, 0.024238229171745118, 0.8255727390752785, 0.14881852068400825, 0.8112558819985408, 0.4890651176168527, 0.8454457169547992, 0.31638859553888377, 0.44291089642901416, 0.5719823755443534, 0.16841544180495382, 0.027626652916785788, 0.7443595070073105, 0.558890537064957, 0.7368327369369875, 0.6620169663786173, 0.5069100456253084, 0.10568051956448299, 0.6049165379895333, 0.8787933693770658, 0.5468255381477648, 0.6283483363395972, 0.022366489475047713, 0.6185225641432791, 0.3117432657775069, 0.19109524865383942, 0.7127230936526828, 0.6688757283653204, 0.09334058924227262, 0.18122247149560333, 0.4056358070065499, 0.8114220220233536, 0.3878142287853389, 0.5761298767098014, 0.7485485532836254, 0.08818513651685944, 0.4282189799887617, 0.9729518275740489, 0.07249340067329868, 0.37242331450013055, 0.6781881918840494, 0.7831663586225461, 0.7929583632543981, 0.15737458204896304, 0.9105366280104408, 0.5456098011028314, 0.35638323962020724, 0.1604159347054014, 0.1368824914953054, 0.4955206638399645, 0.27534009610905263, 0.6486776402026347, 0.4240391764192094, 0.6970823887087281, 0.13231908742648024, 0.026998813309158542, 0.010711682836498593, 0.545733838457532, 0.2628503211167391, 0.005179613440989828, 0.974172279314286, 0.0862305279691401, 0.7572321677592344, 0.34268190601808024, 0.3969763632257125, 0.19104335827805619, 0.22122977608274252, 0.06495797160331362, 0.07457956545542843, 0.799365410133189, 0.6892835885705483, 0.4367905361820239, 0.022039279554333713, 0.09378134015256301, 0.5615393423102526 };


    int randCounter = 0;

    [SerializeField]
    List<NeuralNetworkTest> stupidNets;

    // Start is called before the first frame update
    void Start()
    {

        random = new System.Random(5);
        stupidNets = new List<NeuralNetworkTest>();
        
    }

    public void InitInitialNetworks()
    {
        if ( populationSize % 5 != 0)
        {
            populationSize = number_of_players_per_generation;
        }

        nets = new List<NeuralNetworkTest>();

        for ( int i = 0; i < populationSize; i++)
        {
            NeuralNetworkTest net = new NeuralNetworkTest(layers);
            net.Mutate();
            nets.Add(net);
        }
        

    }

    public void StartGame()
    {
        if ( gameRunning )
        {
            remainingAliveBirds = 0;
            foreach( Player p in players)
            {
                if (p.alive)
                {
                    remainingAliveBirds++;
                    remainingBirdsText.text = "Rem: " + remainingAliveBirds;
                }
            }

            if ( remainingAliveBirds > 0)
            {
                return;
            }
            
            gameRunning = false;

        }
        else
        {
            startGameButton.enabled = false;
            startGameButton.gameObject.SetActive(false);
            timer = 5.1f;
            gameRunning = true;

            if (generationNumber == 0)
            {
                InitInitialNetworks();
            }
            else
            {

                foreach (Player p in players)
                {
                    if (p.Score >= maxScore)
                    {
                        maxScore = p.Score;
                        maxScoreGeneration = generationNumber;
                        maxScoreText.text = "Max Score: " + maxScore + " in Generation: " + maxScoreGeneration;
                    }
                }

                nets.Sort();

                for ( int i = 0; i < populationSize/5; i++)
                {
                    stupidNets.Add(nets[i]);
                }

                for (int i = 0; i < 4 * populationSize / 5; i++)
                {
                    nets[i] = new NeuralNetworkTest(nets[i % (int)(populationSize / 5) + (4 * populationSize / 5)]);
                    nets[i].Mutate();

                    while (stupidNets.Contains(nets[i]))
                    {
                        nets[i].Mutate();
                    }

                }

                for ( int i = 4 * populationSize / 5; i < populationSize; i++)
                {
                    nets[i] = new NeuralNetworkTest(nets[i]);
                }

                //// Randomize to add variations
                //for ( int i = 0; i < 10; i++)
                //{
                //    nets[i] = new NeuralNetworkTest(layers);
                //    //nets[i].Mutate();
                //}
                
                for (int i = 0; i < populationSize; i++)
                {
                    nets[i].SetFitness(0f);
                }
            }

            generationNumber++;
            ClearPipes();
            generationText.text = "Generation: " + generationNumber;
            CreatePlayerBodies();

            // ResetGame();
        }
    }

    void ClearPipes()
    {
        GameObject[] pipes = GameObject.FindGameObjectsWithTag("Pipe");
        foreach ( GameObject p in pipes)
        {
            GameObject.Destroy(p);
        }
        randCounter = 0;
    }

    private void CreatePlayerBodies()
    {
        if ( players != null)
        {
            for ( int i = 0; i < players.Count; i++)
            {
                GameObject.Destroy(players[i].gameObject);
            }
        }

        players = new List<Player>();

        for ( int i = 0; i < populationSize; i++)
        {
            Player player = ((GameObject)Instantiate(playerPrefab, playerList.transform)).GetComponent<Player>();
            player.Init(nets[i]);
            players.Add(player);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!gameFinished)
        {
            StartGame();
            // Let us call it every 5 seconds now.
            if (timer > 5.0f)
            {
                timer = 0.0f;
                if (gameRunning)
                {
                    //SpawnPipesPreloaded();
                    SpawnPipe();
                }
            }

            timer += Time.deltaTime;
            
        }
        else
        {   
            startGameButton.gameObject.SetActive(true);
            startGameButton.enabled = true;
        }

    }

    public void SpawnPipesPreloaded()
    {
        GameObject new_pipe_top = Instantiate(pipePrefab, pipeList.transform);
        GameObject new_pipe_bottom = Instantiate(pipePrefab, pipeList.transform);

        if ( randCounter > randomFloats.Length)
        {
            randCounter = 0;
        }

        float top_pipe_bot_pos = (float)randomFloats[randCounter++] * (max_top - min_top);
        float scale = screenTop - top_pipe_bot_pos;
        float pipe_top_pos = (top_pipe_bot_pos + screenTop) / 2;

        float bot_pipe_top_pos = top_pipe_bot_pos - space_between_top_and_bot;
        float scale_bot = bot_pipe_top_pos - screenBot;
        float pipe_bot_pos = (bot_pipe_top_pos + screenBot) / 2;

        new_pipe_top.transform.position = new Vector3(x_value, pipe_top_pos, -1);
        new_pipe_top.transform.localScale = new Vector3(1, -1 * scale, 1);
        //new_pipe_top.transform.Rotate(new Vector3(0, 180, 0));

        new_pipe_bottom.transform.position = new Vector3(x_value, pipe_bot_pos, -1);
        new_pipe_bottom.transform.localScale = new Vector3(1, scale_bot, 1);
    }

    public void SpawnPipe()
    {

        GameObject new_pipe_top = Instantiate(pipePrefab, pipeList.transform);
        GameObject new_pipe_bottom = Instantiate(pipePrefab, pipeList.transform);

        float top_pipe_bot_pos = (float)random.NextDouble() * (max_top - min_top);
        float scale = screenTop - top_pipe_bot_pos;
        float pipe_top_pos = (top_pipe_bot_pos + screenTop) / 2;

        float bot_pipe_top_pos = top_pipe_bot_pos - space_between_top_and_bot;
        float scale_bot = bot_pipe_top_pos - screenBot;
        float pipe_bot_pos = (bot_pipe_top_pos + screenBot) / 2;
        
        new_pipe_top.transform.position = new Vector3(x_value, pipe_top_pos, -1);
        new_pipe_top.transform.localScale = new Vector3(1, -1 * scale, 1);
        //new_pipe_top.transform.Rotate(new Vector3(0, 180, 0));

        new_pipe_bottom.transform.position = new Vector3(x_value, pipe_bot_pos, -1);
        new_pipe_bottom.transform.localScale = new Vector3(1, scale_bot, 1);

    }

    public void ResetGame()
    {
        // This function just resets the game, and all the Players.
        // Delete all the Pipes.
        GameObject[] pipes = GameObject.FindGameObjectsWithTag("Pipe");
        foreach( GameObject pipe in pipes)
        {
            Destroy(pipe);
        }

        gameRunning = false;
        gameFinished = false;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach ( GameObject player in players)
        {
            player.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
            player.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            player.transform.eulerAngles = Vector3.zero ;
            player.GetComponent<Rigidbody2D>().Sleep();
        }
    }

    // We Create 20 Birds per generation.
    // We Create 20 Neural networks per generation.
    // We Run through the game until all the birds die.
    //      We display the highest score and not all scores.

    // We Sort the Neural Nets.
    // We then Mutate the lower half, clear the fitness scores.
    // We restart the game and use these new Neural networks for the next Generation.

    // Need a way to restart the game without deleting the existing nets.
    //      The Pipes would delete themselves.
    //      The Players need to be reset, with the new neural nets.
    //      The Players score has to be reset.
    //      The Players start again.

    // Need a way to store the best net values before we quit the game.


}
