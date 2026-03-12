export default function LoadingScreen() {
  return (
    <div className="page-container flex items-center justify-center">
      <div className="text-center">
        <div className="w-16 h-16 border-4 border-blue-500 border-t-transparent rounded-full animate-spin mx-auto mb-4" />
        <p className="text-gray-600 font-medium">Loading ...</p>
      </div>
    </div>
  );
}