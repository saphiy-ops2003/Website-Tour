import { 
  SearchIcon, 
  HeartIcon, 
  UserCircleIcon, 
  MapPinIcon, 
  StarIcon, 
  ClockIcon, 
  ChevronRightIcon, 
  ArrowRightIcon,
  ShieldCheckIcon,
  PhoneIcon,
  MailIcon,
  LockIcon,
  CreditCardIcon,
  BadgePercentIcon,
  GlobeIcon
} from 'lucide-react';

export default function Navbar({ currentView, setView }: { currentView: string, setView: (v: string) => void }) {
  return (
    <header className="sticky top-0 w-full z-50 bg-white/80 backdrop-blur-md border-b border-slate-200/50 shadow-sm">
      <nav className="flex justify-between items-center max-w-7xl mx-auto px-6 h-20">
        <div 
          className="text-2xl font-black text-primary-dark tracking-tighter cursor-pointer"
          onClick={() => setView('home')}
        >
          VietExplore
        </div>
        
        <div className="hidden md:flex items-center gap-8 text-sm font-medium">
          <button 
            onClick={() => setView('home')}
            className={`${currentView === 'home' ? 'text-primary border-b-2 border-primary pb-1 font-bold' : 'text-slate-600 hover:text-primary'} transition-all`}
          >
            Trang Chủ
          </button>
          <button 
            onClick={() => setView('tours')}
            className={`${currentView === 'tours' ? 'text-primary border-b-2 border-primary pb-1 font-bold' : 'text-slate-600 hover:text-primary'} transition-all`}
          >
            Danh Sách Tour
          </button>
          <button 
            onClick={() => setView('deals')}
            className={`${currentView === 'deals' ? 'text-primary border-b-2 border-primary pb-1 font-bold' : 'text-slate-600 hover:text-primary'} transition-all`}
          >
            Ưu đãi
          </button>
          <button 
            onClick={() => setView('about')}
            className={`${currentView === 'about' ? 'text-primary border-b-2 border-primary pb-1 font-bold' : 'text-slate-600 hover:text-primary'} transition-all`}
          >
            Giới thiệu
          </button>
        </div>

        <div className="flex items-center gap-4">
          <SearchIcon className="w-5 h-5 text-slate-500 cursor-pointer hover:text-primary transition-colors" />
          <HeartIcon className="w-5 h-5 text-slate-500 cursor-pointer hover:text-primary transition-colors" />
          <div className="flex items-center gap-2 cursor-pointer group" onClick={() => setView('login')}>
             <UserCircleIcon className="w-6 h-6 text-slate-500 group-hover:text-primary transition-colors" />
          </div>
          <button 
            onClick={() => setView('checkout')}
            className="bg-primary-dark text-white px-6 py-2.5 rounded-lg font-semibold text-sm hover:opacity-90 active:scale-95 transition-all shadow-md"
          >
            Đặt ngay
          </button>
        </div>
      </nav>
    </header>
  );
}
