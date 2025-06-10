namespace TLGames.Models
{
    internal class ProductSystemRequirementModel
    {
        public int SystemRequirementId { get; private set; }
        public int ProductId { get; private set; }
        public string MinimumOS { get; private set; }
        public string RecommendedOS { get; private set; }
        public string MinimumProcessor { get; private set; }
        public string RecommendedProcessor { get; private set; }
        public string MinimumMemory { get; private set; }
        public string RecommendedMemory { get; private set; }
        public string MinimumGraphics { get; private set; }
        public string RecommendedGraphics { get; private set; }
        public string MinimumDirectX { get; private set; }
        public string RecommendedDirectX { get; private set; }
        public string MinimumStorage { get; private set; }
        public string RecommendedStorage { get; private set; }

        public ProductSystemRequirementModel() { }

        public ProductSystemRequirementModel(int systemRequirementId, int productId, string minimumOS, string recommendedOS,
                                      string minimumProcessor, string recommendedProcessor, string minimumMemory,
                                      string recommendedMemory, string minimumGraphics, string recommendedGraphics,
                                      string minimumDirectX, string recommendedDirectX, string minimumStorage,
                                      string recommendedStorage)
        {
            SystemRequirementId = systemRequirementId;
            ProductId = productId;
            MinimumOS = minimumOS;
            RecommendedOS = recommendedOS;
            MinimumProcessor = minimumProcessor;
            RecommendedProcessor = recommendedProcessor;
            MinimumMemory = minimumMemory;
            RecommendedMemory = recommendedMemory;
            MinimumGraphics = minimumGraphics;
            RecommendedGraphics = recommendedGraphics;
            MinimumDirectX = minimumDirectX;
            RecommendedDirectX = recommendedDirectX;
            MinimumStorage = minimumStorage;
            RecommendedStorage = recommendedStorage;
        }

        public void SetSystemRequirementId(int id) { SystemRequirementId = id; }
        public void SetProductId(int id) { ProductId = id; }
        public void SetMinimumOS(string os) { MinimumOS = os; }
        public void SetRecommendedOS(string os) { RecommendedOS = os; }
        public void SetMinimumProcessor(string processor) { MinimumProcessor = processor; }
        public void SetRecommendedProcessor(string processor) { RecommendedProcessor = processor; }
        public void SetMinimumMemory(string memory) { MinimumMemory = memory; }
        public void SetRecommendedMemory(string memory) { RecommendedMemory = memory; }
        public void SetMinimumGraphics(string graphics) { MinimumGraphics = graphics; }
        public void SetRecommendedGraphics(string graphics) { RecommendedGraphics = graphics; }
        public void SetMinimumDirectX(string directX) { MinimumDirectX = directX; }
        public void SetRecommendedDirectX(string directX) { RecommendedDirectX = directX; }
        public void SetMinimumStorage(string storage) { MinimumStorage = storage; }
        public void SetRecommendedStorage(string storage) { RecommendedStorage = storage; }
    }
}
