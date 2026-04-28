import { useState } from 'react';
import { motion, AnimatePresence } from 'motion/react';
import { Heart, Star, Clock, Eye } from 'lucide-react';

interface TourCardProps {
  tour: {
    title: string;
    price: number;
    rating: number;
    duration: string;
    img: string;
    badge?: string;
  };
  key?: string | number;
}

export default function TourCard({ tour }: TourCardProps) {
  const [isHovered, setIsHovered] = useState(false);
  const [showTooltip, setShowTooltip] = useState(false);

  return (
    <motion.div 
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      whileHover={{ y: -8 }}
      onMouseEnter={() => setIsHovered(true)}
      onMouseLeave={() => setIsHovered(false)}
      className="bg-white rounded-2xl overflow-hidden shadow-sm hover:shadow-xl transition-all group flex flex-col border border-slate-100 h-full relative"
    >
      {/* Image Container with Quick View */}
      <div className="relative h-56 overflow-hidden">
        <img 
          src={tour.img} 
          alt={tour.title} 
          className="w-full h-full object-cover group-hover:scale-110 transition-all duration-700" 
        />
        
        <AnimatePresence>
          {isHovered && (
            <motion.div 
              initial={{ opacity: 0 }}
              animate={{ opacity: 1 }}
              exit={{ opacity: 0 }}
              className="absolute inset-0 bg-black/40 backdrop-blur-[2px] flex items-center justify-center gap-3 z-10"
            >
              <motion.button 
                initial={{ scale: 0.8, y: 10 }}
                animate={{ scale: 1, y: 0 }}
                className="bg-white text-primary-dark font-bold px-6 py-2.5 rounded-full flex items-center gap-2 hover:bg-primary hover:text-white transition-all shadow-lg"
              >
                <Eye className="w-4 h-4" />
                Quick View
              </motion.button>
            </motion.div>
          )}
        </AnimatePresence>

        {tour.badge && (
          <div className="absolute top-4 left-4 bg-accent text-white px-3 py-1 rounded-full text-[10px] font-bold uppercase tracking-widest z-20">
            {tour.badge}
          </div>
        )}
        
        <button className="absolute top-4 right-4 bg-white/20 backdrop-blur-md p-2 rounded-full text-white hover:bg-white hover:text-red-500 transition-all z-20">
          <Heart className="w-5 h-5" />
        </button>
      </div>

      {/* Content */}
      <div className="p-6 flex flex-col flex-grow">
        <div className="flex justify-between items-start mb-3">
          <h3 className="font-bold text-slate-800 leading-tight group-hover:text-primary transition-colors line-clamp-2">
            {tour.title}
          </h3>
          <div className="flex items-center gap-1 bg-green-50 px-2 py-1 rounded-lg shrink-0 ml-2">
            <Star className="w-3 h-3 text-green-600 fill-green-600" />
            <span className="text-green-700 font-bold text-xs">{tour.rating}</span>
          </div>
        </div>

        {/* Duration with Tooltip */}
        <div className="relative mb-6">
          <div 
            className="flex items-center gap-2 text-slate-500 text-sm cursor-help w-fit"
            onMouseEnter={() => setShowTooltip(true)}
            onMouseLeave={() => setShowTooltip(false)}
          >
            <Clock className="w-4 h-4 text-primary" /> 
            <span className="font-medium">{tour.duration}</span>
          </div>
          
          <AnimatePresence>
            {showTooltip && (
              <motion.div
                initial={{ opacity: 0, y: 5 }}
                animate={{ opacity: 1, y: 0 }}
                exit={{ opacity: 0, y: 5 }}
                className="absolute bottom-full left-0 mb-2 w-48 p-3 bg-slate-900 text-white text-[11px] rounded-xl shadow-xl z-30 pointer-events-none"
              >
                <p className="font-medium text-slate-200">Bao gồm di chuyển, khách sạn và bữa sáng theo chương trình.</p>
                <div className="absolute bottom-[-4px] left-4 w-2 h-2 bg-slate-900 rotate-45" />
              </motion.div>
            )}
          </AnimatePresence>
        </div>

        <div className="mt-auto flex items-center justify-between pt-4 border-t border-slate-50">
          <div>
            <span className="text-[10px] text-slate-400 font-bold uppercase block">Starting from</span>
            <span className="text-2xl font-black text-primary">${tour.price}</span>
          </div>
          <button className="bg-primary text-white px-5 py-2.5 rounded-xl font-bold text-xs hover:bg-primary-dark transition-all transform hover:scale-105 active:scale-95 shadow-md hover:shadow-lg">
            Details
          </button>
        </div>
      </div>
    </motion.div>
  );
}
