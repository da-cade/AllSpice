<template>
  <span class="navbar-text">
    <div
      class="hoverable text-success"
      @click="login"
      v-if="!user.isAuthenticated"
    >
      <i class="mdi mdi-logout"></i>
    </div>
    <div v-else class="hoverable text-danger" @click="logout">
      <i class="mdi mdi-logout"></i>
    </div>
  </span>
</template>


<script>
import { computed } from "@vue/reactivity";
import { AppState } from "../AppState";
import { AuthService } from "../services/AuthService";
export default {
  setup() {
    return {
      user: computed(() => AppState.user),
      account: computed(() => AppState.account),
      async login() {
        AuthService.loginWithPopup();
      },
      async logout() {
        AuthService.logout({ returnTo: window.location.origin });
      },
    };
  },
};
</script>


<style lang="scss" scoped>
i {
  font-size: 20pt;
}
svg {
  font-size: 20pt;
}
.hoverable {
  cursor: pointer;
}
</style>
