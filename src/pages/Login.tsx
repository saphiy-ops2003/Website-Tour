import { motion } from 'motion/react';
import { 
  User, 
  Lock, 
  Eye, 
  Facebook, 
  Apple, 
  Compass
} from 'lucide-react';

export default function Login({ setView }: { setView: (v: string) => void }) {
  return (
    <div className="min-h-screen flex items-center justify-center relative overflow-hidden font-sans bg-slate-900">
      {/* Background */}
      <div className="absolute inset-0 z-0">
        <img 
          src="https://lh3.googleusercontent.com/aida-public/AB6AXuD0XcUYC6MCPhiKsd2nacmTbDfAMsZg2srpla_Sok9lRvIqE6cDwaKMI6Fxn90wF1c8yTr6Le7sE-eyRWFlxCSIO3mmvq19He_i4fOp3dXTu-uBy1fYJS6hApA-5G1D6r8P8C3DHas9YU1eXV9JPuqkWmiLtaKnln3SK5o5rPE_Upul61Y3kVu_RF-BE3owx4sV_WiC5IeX1zxSLLWYjCA8jB1nOMZjl2867V6lhRnuNxKjvQfZfqcibudFQG4-U78wYcNXz1kifw" 
          alt="Background" 
          className="w-full h-full object-cover opacity-60"
        />
        <div className="absolute inset-0 bg-gradient-to-r from-primary-dark/60 to-transparent"></div>
      </div>

      <motion.div 
        initial={{ opacity: 0, scale: 0.95 }}
        animate={{ opacity: 1, scale: 1 }}
        className="relative z-10 w-full max-w-[1000px] grid grid-cols-1 md:grid-cols-2 bg-white/10 backdrop-blur-md rounded-[2rem] overflow-hidden shadow-2xl border border-white/20 mx-4"
      >
        {/* Left: Welcome */}
        <div className="hidden md:flex flex-col justify-end p-12 text-white">
          <div className="mb-8">
            <span className="text-sm font-bold uppercase tracking-widest text-primary-light mb-4 block">Khám phá Việt Nam</span>
            <div className="flex items-center gap-3 mb-4">
              <Compass className="w-10 h-10 text-white" />
              <h1 className="text-5xl font-bold tracking-tighter">VietExplore</h1>
            </div>
            <p className="text-lg text-white/90 max-w-sm">Hành trình trải nghiệm vẻ đẹp tiềm ẩn của dải đất hình chữ S bắt đầu từ đây.</p>
          </div>
        </div>

        {/* Right: Form */}
        <div className="bg-white p-10 md:p-16 flex flex-col justify-center">
          <div className="mb-10">
            <div className="flex items-center gap-2 mb-6 md:hidden">
               <Compass className="text-primary w-8 h-8" />
               <span className="text-2xl font-black text-primary">VietExplore</span>
            </div>
            <h2 className="text-3xl font-bold text-primary-dark mb-2">Chào mừng trở lại</h2>
            <p className="text-slate-500">Vui lòng đăng nhập để tiếp tục hành trình của bạn.</p>
          </div>

          <form className="space-y-6" onSubmit={(e) => e.preventDefault()}>
            <div className="space-y-2">
              <label className="text-xs font-bold text-slate-500 uppercase tracking-widest block ml-1">Email hoặc Số điện thoại</label>
              <div className="relative">
                <User className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 w-5 h-5" />
                <input 
                  type="text" 
                  className="w-full pl-12 pr-4 py-4 rounded-xl bg-slate-50 border border-slate-200 focus:border-primary focus:ring-1 focus:ring-primary outline-none transition-all" 
                  placeholder="name@example.com" 
                />
              </div>
            </div>

            <div className="space-y-2">
              <div className="flex justify-between items-center px-1">
                <label className="text-xs font-bold text-slate-500 uppercase tracking-widest">Mật khẩu</label>
                <span className="text-xs font-bold text-primary hover:underline cursor-pointer">Quên mật khẩu?</span>
              </div>
              <div className="relative">
                <Lock className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 w-5 h-5" />
                <input 
                  type="password" 
                  className="w-full pl-12 pr-12 py-4 rounded-xl bg-slate-50 border border-slate-200 focus:border-primary focus:ring-1 focus:ring-primary outline-none transition-all" 
                  placeholder="••••••••" 
                />
                <button type="button" className="absolute right-4 top-1/2 -translate-y-1/2 text-slate-400 hover:text-primary transition-colors">
                  <Eye className="w-5 h-5" />
                </button>
              </div>
            </div>

            <button 
              onClick={() => setView('home')}
              className="w-full py-4 bg-primary-dark text-white rounded-xl font-bold shadow-lg hover:shadow-primary/30 transition-all hover:-translate-y-0.5 active:scale-95"
            >
              Đăng nhập
            </button>
          </form>

          <div className="relative my-10 text-center">
            <div className="absolute inset-0 flex items-center">
               <div className="w-full border-t border-slate-100"></div>
            </div>
            <span className="relative bg-white px-4 text-xs font-bold text-slate-400 uppercase tracking-widest">Hoặc đăng nhập bằng</span>
          </div>

          <div className="grid grid-cols-3 gap-4 mb-10">
            <button className="flex items-center justify-center py-3 border border-slate-200 rounded-xl hover:bg-slate-50 transition-colors">
              <img src="https://lh3.googleusercontent.com/aida-public/AB6AXuCAy7iznNneU6jjecqOg0IxKEuOsXmlxrazEzX1vLu52ypf4_cdgrbEzwaVGCsxBNdbAT3Wh0G1BupTOscx4Ds04TSp7oPeu8Y1DShRcdd4nlkN_nrdfkkeMiPnn9lToge1HwiCf78F_qw5mIEeo6QHlpEeC7EtoHSV6INJ9vKsFiiudss4i06wLhS-NkP3VSV0wggTlIdC3huKMuXJEnY1e9lvIxtPbMV2UPcSumr9Gc5cmi_X-UXVKqGQLq8irgeguUMnvKPi-g" className="w-6 h-6" alt="google" />
            </button>
            <button className="flex items-center justify-center py-3 border border-slate-200 rounded-xl hover:bg-slate-50 transition-colors text-[#1877F2]">
              <span className="text-2xl font-black">f</span>
            </button>
            <button className="flex items-center justify-center py-3 border border-slate-200 rounded-xl hover:bg-slate-50 transition-colors text-black">
               <Apple className="fill-black w-6 h-6" />
            </button>
          </div>

          <p className="text-center text-slate-600">
            Chưa có tài khoản? <span className="text-primary font-bold cursor-pointer hover:underline" onClick={() => setView('register')}>Đăng ký ngay</span>
          </p>
        </div>
      </motion.div>
    </div>
  );
}
