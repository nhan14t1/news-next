import { useEffect, useState } from 'react';
import { get } from '../../shared/utils/apiUtils';

function useComboFetch() {
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(false);
  const [vietNamPosts, setVietNamPosts] = useState([]);
  const [globalPosts, setGlobalPosts] = useState([]);
  const [videoPosts, setVideoPosts] = useState([]);
  const [topPosts, setTopPosts] = useState([]);

  useEffect(() => {
    setLoading(true);
    setError(false);

    get('/post/home-page').then(res => {
      if (res && res.data) {
        const { vietNamPosts, globalPosts, videoPosts, topPosts } = res.data;
        setVietNamPosts(vietNamPosts);
        setGlobalPosts(globalPosts);
        setVideoPosts(videoPosts || []);
        setTopPosts(topPosts || []);
      }
    }).finally(() => {
      setLoading(false);
    });
  }, []);

  return { loading, error, vietNamPosts, globalPosts, videoPosts, topPosts };
}

export default useComboFetch;
